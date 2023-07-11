using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Passaro : MonoBehaviour
{

	//passaro
	public CircleCollider2D passaroCol;
	private Rigidbody2D passaroRB;
	public LayerMask passaroLayer;
	public float tempoParaMorrer = 5.0f;
	public bool estaMorrendo = false;
	public bool estouAtivo = false;
	public float initialDistance;
	public bool passaroLancado;

	//Control Input
	public bool clicked;
	private Touch touch;
	private Vector3 clickPosition;

	public float distanceDragged;
	private Ray rayo;

	// Audio e efeitos
	public GameObject efeitoDeMorte;
	public AudioClip audioPassaro;
	public AudioClip audioMortePassaro;

	//Rastro
	private TrailRenderer rastro;
	
	bool interruptedMovement;
	private void Awake()
	{
		passaroCol = GetComponent<CircleCollider2D>();
		passaroRB = GetComponent<Rigidbody2D>();
		rastro = GetComponentInChildren<TrailRenderer>();
		
	}
	private void Start()
	{
		initialDistance = Vector2.Distance(Catapult.instance.GetPosition(), GameObject.FindWithTag( Constants.POSINICIAL_TAG ).GetComponent<Transform>().position);
		rayo = new Ray(GameManager.instance.posInicial.position, Vector3.zero);

	}
	// Update is called once per frame
	void Update()
	{
		//Comprobamos se o jogo não foi pausado
		if (!GameManager.instance.pausado && GameManager.instance.jogoComecou )
		{
			//Se de fato o passaro tem seu corpo rigido
#if UNITY_ANDROID
			// Se não tinha sido clickado e tem um touch em cima do passaro
			
			if ( Input.touchCount > 0 )
			{
				touch = Input.GetTouch( 0 );
				if(!clicked)
				{
					if(touch.phase == TouchPhase.Began )
					{
						if ( ClickedOnCatapult( touch.position ) )
						{
							ActivarDragPassaro();
						}
					}
				}
				else if(clicked)
				{
					DraggingWithTouch();
				}
					
			}
#endif	
#if UNITY_EDITOR
			// Ativamos com mouse o drag só que aqui é gerenciado pelos moétodos OnMouseUp e OnMouseDown 
			if ( clicked && Input.touchCount == 0)
			{
				DraggingWithMouse();
			}
#endif
			// Verificamos que o passaro tenha sido lançado, não esteja morrendo e seja este o ativo para ativar sua morte. 
			if ( passaroLancado && !estaMorrendo && estouAtivo )
			{
				MataPassaro();
			}

			//Quando é lançado, a camera segue o passaro até ele começar morrer (dá para melhorar)
			if ( passaroRB.isKinematic == false && !estaMorrendo && !interruptedMovement)
			{
				SeguePassaro();
			}
		
		}
	}
	private void OnCollisionEnter2D( Collision2D collision )
	{
		interruptedMovement = true;
		StartCoroutine(SomeDelay(2f,continueToRecharge));
	}
	void OnMouseDown()
	{
		bool estaNaEstilingue = transform.position == GameManager.instance.posInicial.position;
		if ( estaNaEstilingue  && Input.touchCount == 0)
		{
			if ( ClickedOnCatapult( Input.mousePosition ) )
			{
				ActivarDragPassaro();
			}
		}
	}
	// Desactiva o drag atual
	void OnMouseUp()
	{
		if ( estouAtivo && Input.touchCount == 0)
		{
			DesactivarDragPassaro();
		}
	}
	// Pegar moedas
	private void OnTriggerEnter2D( Collider2D collision )
	{
		if ( collision.gameObject.CompareTag( Constants.MOEDA_TAG ) )
		{
			CoinManager.instance.moneyDisplay.AddTemporalMoney(50);
			Destroy(collision.gameObject);
		}
	}
	private bool ClickedOnCatapult(Vector3 inputPos)
	{
		Vector3 wp = Camera.main.ScreenToWorldPoint( inputPos );
		RaycastHit2D hitCatapult = Physics2D.Raycast( wp, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Catapult"));
		if ( hitCatapult.collider == Catapult.instance.colisorCompleto && GameManager.instance.passaroName == gameObject.name)
		{
			clickPosition = wp;
			if ( !clicked && !passaroLancado )
			{
				//se esta tudo certo ativamos o drag do passaro
				return true;
			}
		}
		return false;
	}
	private void SeguePassaro()
	{
		Vector3 posCam = Camera.main.transform.position;
		posCam.x = transform.position.x;
		posCam.x = Mathf.Clamp( posCam.x, GameManager.instance.CameraE.position.x, GameManager.instance.CameraD.position.x );
		Camera.main.transform.position = posCam;
	}
	public void continueToRecharge()
	{
		if ( passaroLancado )
		{
			GameManager.instance.passaroCarregado = false;
			Camera.main.GetComponent<CameraSegue>().inPlace = false;
		}
	}
	delegate void func();
	IEnumerator SomeDelay(float t, func callback)
	{
		yield return new WaitForSeconds( t );
		callback();
	}
	//Ativa o drag se o este passaro esta na estilingue
	
	//Multiplica o prefab que será distribuido ao longo da predição da trajetoria, é necessario ter um objeto vazio na cena com a tag dogs para nela inserir todos esses prefabs.
	void CalculatePosition(Vector3 inputPosition)
	{
		Vector3 direcaoRelativa = (inputPosition - clickPosition).normalized;
		rayo.direction = direcaoRelativa;
		Vector3 posFinal;
		distanceDragged = Vector2.Distance(inputPosition, clickPosition);
		print(distanceDragged);
		if(distanceDragged > Catapult.instance.maxDistanceDragged )
		{
			posFinal = rayo.GetPoint(Catapult.instance.maxDistanceDragged);
			distanceDragged = Catapult.instance.maxDistanceDragged;
		}
		else
		{
			posFinal = rayo.GetPoint(distanceDragged);
					
		}
		posFinal.z = 0;
				
		Collider2D bateComABase = Physics2D.OverlapCircle(posFinal,passaroCol.radius * transform.localScale.y, LayerMask.GetMask("PassaroBlock"));
		if(bateComABase == null) 
		{
			transform.position = posFinal;
		}
		//Atualizamos os line Renderer ou elastico da estilingue
		Catapult.instance.LineUpdate();

		//Ativamos o objeto mira se ele esta ativo
		if ( GameManager.instance.mira )
		{
			Mira.instance.Mirando(PegaForca(),passaroRB.mass,transform.position); 
		}
	}
	
	void DraggingWithTouch()
	{
		//Conferimos se o passaro é candidato para ser lançado
		if ( passaroRB.isKinematic )
		{
			//Se desde o primeiro toque foi arrastrado
			if ( touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved )
			{	
				Vector3 touchPos = Camera.main.ScreenToWorldPoint( new Vector3( touch.position.x, touch.position.y, 0 ) );
				CalculatePosition( touchPos );
			}
			// Se parou o touch, desactivamos o drag
			if ( touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
			{
				DesactivarDragPassaro();
			}
		}
	}
	//Mesmo que DragWithTouch 
	void DraggingWithMouse()
	{
		//print("dragging with mouse");
		if ( passaroRB.isKinematic )
		{
			Vector3 mouseWp = Camera.main.ScreenToWorldPoint( Input.mousePosition );
			CalculatePosition(mouseWp);
		}
	}
	// Inicio do Drag, configuramos cada variavel necessaria para o fluxo ser correto
	void ActivarDragPassaro()
	{
		
		if (GameManager.instance.pausado == false )
		{
			//clicked ativa o Drag no método update
			clicked = true;
			// o rastro é desativado em quanto se esta esticando a corda para não ficar aparecendo
			rastro.enabled = false;
			// aqui indicamos que este foi o passaro atingido
			estouAtivo = true;
			// informamos ao game manager para ele ter noção do que ta acontecendo
			GameManager.instance.passaroInDragging = true;		
		}
	}
	// Calculo da força com que sera lançado o passaro
	Vector2 PegaForca()
	{
		float deformacao = distanceDragged - initialDistance;
		Vector2 direcao = (transform.position - Catapult.instance.GetPosition()).normalized;
        // Calcular a força aplicada pela mola como um Vector2
        Vector2 force = - Catapult.instance.constanteResorte * deformacao * direcao;
		return force;
	}

	// Desativamos o drag 
	void DesactivarDragPassaro()
	{
		//Se não esta solto o passaro
		if ( passaroRB.isKinematic )
		{
			// o esticamento minimo para ser considerado um lançamento
			if(distanceDragged > Catapult.instance.minDistanceDragged )
			{
				//para ser jogado deixamos de fazer o passaro kinematico se tornando dinamico.
				passaroRB.isKinematic = false;
				//conferimos se existe um rastro para este passaro
				if ( rastro != null )
				{
					//ativamos o rastro que agora se queremos visualizar
					rastro.enabled = true;
					// se não habia rastro guardamos a referencia deste no game manager
					if(GameManager.instance.rastroPrevio == null )
					{
						GameManager.instance.rastroPrevio = rastro;
					}
					//Se tinha matamos o guardado antigamente e guardamos o atual
					else
					{
						GameManager.instance.rastroPrevio.GetComponent<MataRastro>().MataRastroPassaro();
						GameManager.instance.rastroPrevio = rastro;
					}
			
				}
				// aqui aplicamos a forca de lançamento
				passaroRB.AddForce(PegaForca(), ForceMode2D.Impulse);
				passaroLancado = true;
				GameManager.instance.passarosUsados++;
				AudioManager.instance.PlayEffect(audioPassaro);
				Catapult.instance.LineRendererDisabled();
				
				//Se estava ativa a mira executamos sua substração
				if (GameManager.instance.mira)
				{
					UsableObjectsManager.instance.ObjectUsed(UsableObjectsEnum.Mira);
				}
			}
			//Se não foi atingido o esticamento minimo, reiniciamos as variaveis.
			else
			{
				estouAtivo = false;
				transform.position = GameManager.instance.posInicial.position;
				Catapult.instance.LineUpdate();
			}
			GameManager.instance.passaroInDragging = false;
			clicked = false;
			Mira.instance.desligarCaminho();
		}
		
	}
	//----------------Morte Passaro------------------------
	//método executado no update que confere se o passaro não esta mais se movimentando para ativar sua morte
	public void MataPassaro()
	{
		if ( passaroRB.velocity.magnitude == 0 && passaroRB.IsSleeping())
		{
			estaMorrendo = true;

			StartCoroutine( TempoMorte() );
			//print( "inicia morte" );
		}
	}
	// morte do passaro, reinicio e configurações para manter o fluxo
	public IEnumerator TempoMorte()
	{
		yield return new WaitForSeconds( tempoParaMorrer );
		Instantiate( efeitoDeMorte, new Vector3( transform.position.x, transform.position.y, -0.5f ), Quaternion.identity );
		AudioManager.instance.PlayEffect(audioMortePassaro);
		continueToRecharge();
		Destroy( gameObject );
		//print( "Morreu" );
	}
	//-----------------------------------------------------
	
}
