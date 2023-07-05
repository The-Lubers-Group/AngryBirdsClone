using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Drag : MonoBehaviour
{

	//passaro
	private CircleCollider2D passaroCol;
	private Rigidbody2D passaroRB;
	public LayerMask passaroLayer;
	public float tempoParaMorrer = 5.0f;
	public bool estaMorrendo = false;
	public bool estouAtivo = false;
	public bool picked = false;

	//Control Input
	private bool clicked;
	private Touch touch;
	private Vector3 clickPosition;
	//Estilingue ou catapulta
	[SerializeField] private float frequencia = 13.0f;
	private Transform catapult;
	private PolygonCollider2D catapultCol;
	public LineRenderer lineFront;
	public LineRenderer lineBack;
	private Ray leftCatapultRay;
	public float valorCondLimiteX = -6.3f;
	public float valorCondLimiteY = -4f;
	private Vector2 catapultToBird;
	public Rigidbody2D catapultRB;
	private Ray rayToMT;
	[SerializeField] private float esticamentoMin = 1.0f;

	// Audio e efeitos
	public GameObject efeitoDeMorte;
	public AudioSource audioPassaro;
	public GameObject audioMortePassaro;

	//Rastro
	private TrailRenderer rastro;
	
	//Mira
	private bool mirando = false;
	private GameObject dotsContainer;
	[SerializeField] private GameObject dotPrefab;
	private List<GameObject> caminho;
	private List<Renderer> caminhoRender = new List<Renderer>();
	private float initialDistance;
	public int pointsNum = 30;


	Vector3 prevCamPos;
	bool interruptedMovement;
	private void Awake()
	{
		lineFront = GameObject.FindWithTag( "CatapultLF" ).GetComponent<LineRenderer>();
		GameObject ctp = GameObject.FindWithTag( "CatapultLB" );
		lineBack = ctp.GetComponent<LineRenderer>();
		catapultRB = ctp.GetComponent<Rigidbody2D>();
		catapultCol = ctp.GetComponent<PolygonCollider2D>();

		passaroCol = GetComponent<CircleCollider2D>();
		passaroRB = GetComponent<Rigidbody2D>();
		rastro = GetComponentInChildren<TrailRenderer>();
		audioPassaro = GetComponent<AudioSource>();
	
		catapult = catapultRB.transform;
		leftCatapultRay = new Ray( lineBack.transform.position, Vector3.zero );
		rayToMT = new Ray( catapult.position, Vector3.zero );
		
		StartMira();
	}

	// Update is called once per frame
	void Update()
	{
		//Comprobamos se o jogo não foi pausado
		if (!GameManager.instance.pausado && GameManager.instance.jogoComecou )
		{
			//Se de fato o passaro tem seu corpo rigido
			if ( passaroRB != null )
			{
#if UNITY_ANDROID
				// Se não tinha sido clickado e tem um touch em cima do passaro
				if ( Input.touchCount > 0 && !picked)
				{
					touch = Input.GetTouch( 0 );
					if(!clicked)
					{
						Vector3 wp = Camera.main.ScreenToWorldPoint( touch.position );
						wp.z = 0;
						RaycastHit2D hitCatapult = Physics2D.Raycast( wp, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Catapult"));

						if ( hitCatapult.collider == catapultCol && GameManager.instance.passaroName == gameObject.name)
						{
							clickPosition = wp;
							print(clickPosition);
							if ( !clicked && !GameManager.instance.passaroLancado )
							{
								//se esta tudo certo ativamos o drag do passaro
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
				if ( GameManager.instance.passaroLancado && !estaMorrendo && estouAtivo )
				{
					MataPassaro();
				}

				//Quando é lançado, a camera segue o passaro até ele começar morrer (dá para melhorar)
				if ( passaroRB.isKinematic == false && !estaMorrendo && !interruptedMovement)
				{
					Vector3 posCam = Camera.main.transform.position;
					posCam.x = transform.position.x;
					posCam.x = Mathf.Clamp( posCam.x, GameManager.instance.CameraE.position.x, GameManager.instance.objD.position.x );
					Camera.main.transform.position = posCam;
				}
			}
		}
	}
	private void OnCollisionEnter2D( Collision2D collision )
	{
			interruptedMovement = true;
	}
	//Ativa o drag se o este passaro esta na estilingue
	void OnMouseDown()
	{
		bool estaNaEstilingue = transform.position == GameManager.instance.posInicial.position;
		if ( estaNaEstilingue )
		{
			ActivarDragPassaro();
		}
	}
	// Desactiva o drag atual
	void OnMouseUp()
	{
		if ( estouAtivo )
		{
			DesactivarDragPassaro();
		}
	}
	// Pegar moedas
	private void OnTriggerEnter2D( Collider2D collision )
	{
		if ( collision.gameObject.CompareTag( "moeda" ) )
		{
			GameManager.instance.moedas += 50;
			UIManager.instance.moedas.text = GameManager.instance.moedas.ToString();
			Destroy(collision.gameObject);
		}
	}
	//Multiplica o prefab que será distribuido ao longo da predição da trajetoria, é necessario ter um objeto vazio na cena com a tag dogs para nela inserir todos esses prefabs.
	void StartMira()
    {
        dotsContainer = GameObject.FindWithTag("dots");

		if (dotsContainer.transform.Cast<Transform>().Count() == 0)
		{
			for ( int i = 0; i < pointsNum; i++ )
			{
				GameObject local = Instantiate(dotPrefab, Vector3.zero, Quaternion.identity, dotsContainer.transform);
				Renderer render = local.GetComponent<Renderer>();
				caminhoRender.Add(render);
				render.enabled = false;
			}
			caminho = dotsContainer.transform.Cast<Transform>().ToList().ConvertAll(t => t.gameObject);
		} else
		{
			caminho = dotsContainer.transform.Cast<Transform>().ToList().ConvertAll(t => t.gameObject);
			for ( int i = 0; i < pointsNum; i++ )
			{
				Renderer render = caminho[i].GetComponent<Renderer>();
				caminhoRender.Add(render );
				render.enabled = false;
			}
		}
		initialDistance = Vector2.Distance(catapultRB.transform.position, GameObject.FindWithTag( "posInicial" ).GetComponent<Transform>().position);
    }
	public void SetupLine1(Vector3 point)
	{
		//Setando o começo das linhas nos pivot da estilingue
		lineFront.SetPosition( 1, point );
		lineBack.SetPosition( 1, point );
	}
	public void LineUpdate()
	{
			//Obtendo a Direção de Estilingue até o Passaro
			catapultToBird = transform.position - lineBack.transform.position;

			//Asignando a direção que esta apontando para o passaro
			leftCatapultRay.direction = catapultToBird;
			//Obtendo do Raio um ponto desse raio, somando a distancia que leva desde a estilingue até o passaro e adicionamos o raio do seu colisor para cobrir o passaro completamente.  
			Vector3 pointL = leftCatapultRay.GetPoint( catapultToBird.magnitude + (passaroCol.radius * transform.localScale.y));
			pointL.z = 0;
			//Atualiza posição da linha com respeito a posição do passaro.
			SetupLine1(pointL);
	}
	void DraggingWithTouch()
	{
		//Conferimos se o passaro é candidato para ser lançado
		if ( passaroRB.isKinematic )
		{
			//Se desde o primeiro toque foi arrastrado
			if ( touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved )
			{	
				//Ativamos o objeto mira se ele esta ativo
				if ( GameManager.instance.mira )
				{
					Mirando(); 
				}
				Vector3 tPos = Camera.main.ScreenToWorldPoint( new Vector3( touch.position.x, touch.position.y, 10 ) );
				catapultToBird = tPos - catapult.position;
				if ( catapultToBird.sqrMagnitude > 9.0f )
				{
					rayToMT.direction = catapultToBird;
					tPos = rayToMT.GetPoint( 3f );
				}
				if(tPos.y < valorCondLimiteY )
				{
					tPos.y = valorCondLimiteY;
				}
				if(tPos.x > valorCondLimiteX)
				{
					tPos.x = valorCondLimiteX;
				}
				
				transform.position = tPos;
				//Atualizamos os line Renderer ou elastico da estilingue
				LineUpdate();
			}
			// Se parou o touch, desactivamos o drag
			if ( touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled )
			{

				DesactivarDragPassaro();
				picked = false;
			}
		}
	}
	//Mesmo que DragWithTouch 
	void DraggingWithMouse()
	{
		//print("dragging with mouse");
		
		if ( passaroRB.isKinematic )
		{
			if ( GameManager.instance.mira )
			{
				Mirando(); 
			}
			//ShowTrajectory();
			Vector3 mouseWp = Camera.main.ScreenToWorldPoint( Input.mousePosition );
			

			catapultToBird = mouseWp - catapult.position;
			if ( catapultToBird.sqrMagnitude > 9.0f )
			{
				rayToMT.direction = catapultToBird;
				mouseWp = rayToMT.GetPoint( 3f );
			}
			if(mouseWp.y < valorCondLimiteY)
			{
				mouseWp.y = valorCondLimiteY;
			}
			if(mouseWp.x > valorCondLimiteX)
			{
				mouseWp.x = valorCondLimiteX;
			}
			mouseWp.z = 0f;
			transform.position = mouseWp;
			LineUpdate();
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
		float currentDistance = Vector2.Distance(transform.position, catapultRB.transform.position);
		float deformacao = currentDistance - initialDistance;
		Vector2 direcao = (transform.position - catapultRB.transform.position).normalized;
        // Calcular a força aplicada pela mola como um Vector2
        Vector2 force = - frequencia * deformacao * direcao;
		return force;
	}

	// Desativamos o drag 
	void DesactivarDragPassaro()
	{
		//Se não esta solto o passaro
		if ( passaroRB.isKinematic )
		{
			// 0.4 é o esticamento minimo para ser considerado um lançamento
			if(catapultToBird.sqrMagnitude > esticamentoMin )
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
				
				GameManager.instance.passaroLancado = true;
				audioPassaro.Play();
				//Se estava ativa a mira executamos sua substração
				if (GameManager.instance.mira)
				{
					UsableObjectsManager.instance.ObjectUsed(UsableObjectsEnum.Mira);
				}
				lineBack.enabled = false;
				lineFront.enabled = false;
			}
			//Se não foi atingido o esticamento minimo, reiniciamos as variaveis.
			else
			{
				estouAtivo = false;
				transform.position = GameManager.instance.posInicial.position;
				LineUpdate();
				mirando = false;
			}
			GameManager.instance.passaroInDragging = false;
			clicked = false;
			desligarCaminho();

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
		estouAtivo = false;
		Instantiate( audioMortePassaro, new Vector3( transform.position.x, transform.position.y, -0.5f ), Quaternion.identity );

		Destroy( gameObject );
		GameManager.instance.passarosUsados++;
		GameManager.instance.passaroCarregado = false;
		GameManager.instance.passaroLancado = false;
		Camera.main.GetComponent<CameraSegue>().inPlace = false;
		estaMorrendo = false;
		//print( "Morreu" );
	}
	//-----------------------------------------------------
	

	// Metodos do objeto Mira
	void ligarCaminho()
	{
		foreach ( Renderer item in caminhoRender )
		{
			item.enabled = true;
		}
	}
	void desligarCaminho()
	{
		foreach ( Renderer item in caminhoRender )
		{
			item.enabled = false;
		}
	}
	Vector2 CaminhoPonto(Vector2 posInicial,Vector2 velInicial,float tempo)
	{
		return posInicial + velInicial * tempo + 0.5f * Physics2D.gravity * tempo * tempo;
	}
	void CalculoCaminho()
	{
			Vector2 vel = PegaForca() / passaroRB.mass;
			for(int x=0; x < pointsNum; x++)
			{
				float t = (x * Time.fixedDeltaTime * 12);
				Vector3 point = CaminhoPonto (transform.position, vel, t);
				point.z = 0f;
				caminho [x].transform.position = point;
				
			}
		
	}
	public void Mirando()
	{
		if(Input.GetMouseButton(0) && !GameManager.instance.passaroLancado)
		{
			//print("Apreto el mouse");
			if (mirando == false) {
				ligarCaminho();
				mirando = true;
				CalculoCaminho ();
			} else {
				//print("mirando es verdadero");
				CalculoCaminho ();
			}
		}
	}
}
