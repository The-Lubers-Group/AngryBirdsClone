using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Drag : MonoBehaviour
{
	private Collider2D drag;
	public LayerMask layer;
	[SerializeField] private bool clicked;
	private Touch touch;
	public float TempoParaMorrer = 5.0f;
	public bool EstaMorrendo = false;
	public LineRenderer lineFront;
	public LineRenderer lineBack;

	private Ray leftCatapultRay;
	private CircleCollider2D passaroCol;
	private Vector2 catapultToBird;
	private Vector3 pointL;

	private SpringJoint2D spring;
	private Vector2 prevVel;
	private Rigidbody2D passaroRB;

	public GameObject bomb;
	public AudioSource audioPassaro;
	public GameObject audioMortePassaro;
	//Limite do elastico

	private Transform catapult;
	private Ray rayToMT;

	//Rastro
	private TrailRenderer rastro;

	public Rigidbody2D catapultRB;
	public bool EstouPronto = false;
	public bool picked = false;
	private void Awake()
	{
		lineFront = GameObject.FindWithTag( "CatapultLF" ).GetComponent<LineRenderer>();
		lineBack = GameObject.FindWithTag( "CatapultLB" ).GetComponent<LineRenderer>();
		catapultRB = GameObject.FindWithTag( "CatapultLB" ).GetComponent<Rigidbody2D>();

		drag = GetComponent<Collider2D>();
		passaroCol = GetComponent<CircleCollider2D>();
		passaroRB = GetComponent<Rigidbody2D>();
		spring = GetComponent<SpringJoint2D>();
		rastro = GetComponentInChildren<TrailRenderer>();
		audioPassaro = GetComponent<AudioSource>();

		spring.connectedBody = catapultRB;
		catapult = spring.connectedBody.transform;
		leftCatapultRay = new Ray( lineFront.transform.position, Vector3.zero );
		rayToMT = new Ray( catapult.position, Vector3.zero );

	}
	void Start()
	{
		SetupLine();
	}

	// Update is called once per frame
	void Update()
	{
		if (GameManager.instance.pausado == false )
		{
			LineUpdate();
			SpringEffect();
		
			if ( passaroRB != null )
			{
				prevVel = passaroRB.velocity;

	#if UNITY_ANDROID

				if ( Input.touchCount > 0 && !picked)
				{
					touch = Input.GetTouch( 0 );

					Vector2 wp = Camera.main.ScreenToWorldPoint( touch.position );
					RaycastHit2D hit = Physics2D.Raycast( wp, Vector2.zero, Mathf.Infinity, layer.value );
					if ( hit.collider == drag && GameManager.instance.nomePassaro == gameObject.name)
					{
						if ( !clicked )
						{
							ActivarDrag();	
						}
						
					}
					if( clicked )
					{
						DraggingWithTouch();
					}
					

				}

	#endif

	#if UNITY_EDITOR
				if ( clicked && Input.touchCount == 0)
				{
					DraggingWithMouse();
				}
	#endif
				if ( GameManager.instance.passaroLancado && !EstaMorrendo && EstouPronto )
				{
					MataPassaro();
				}

				if ( passaroRB.isKinematic == false )
				{
					Vector3 posCam = Camera.main.transform.position;
					posCam.x = transform.position.x;
					posCam.x = Mathf.Clamp( posCam.x, GameManager.instance.objE.position.x, GameManager.instance.objD.position.x );
					Camera.main.transform.position = posCam;
				}
			}
		}
	}
	void SetupLine()
	{
		//Setando o começo das linhas nos pivot da estilingue
		lineFront.SetPosition( 0, lineFront.transform.position );
		lineBack.SetPosition( 0, lineBack.transform.position );

	}
	void LineUpdate()
	{
		if ( transform.name == GameManager.instance.nomePassaro )
		{
			//Obtendo a Direção de Estilingue até o Passaro
			catapultToBird = transform.position - lineFront.transform.position;


			//Asignando a direção que esta apontando para o passaro
			leftCatapultRay.direction = catapultToBird;

			//Obtendo do Raio um ponto desse raio, somando a distancia que leva desde a estilingue até o passaro e adicionamos o raio do seu colisor para cobrir o passaro completamente.  
			pointL = leftCatapultRay.GetPoint( catapultToBird.magnitude + passaroCol.radius );

			//Atualiza posição da linha com respeito a posição do passaro.
			lineFront.SetPosition( 1, pointL );
			lineBack.SetPosition( 1, pointL );
		}

	}

	void SpringEffect()
	{
		if ( spring != null && GameManager.instance.passaroCarregado )
		{
			if ( passaroRB.isKinematic == false )
			{
				//verifico velocidade
				if ( prevVel.sqrMagnitude > passaroRB.velocity.sqrMagnitude )
				{
					lineBack.enabled = false;
					lineFront.enabled = false;
					Destroy( spring );
					//ajuste da velocidade para corregir o efeto do spring
					passaroRB.velocity = prevVel;
				}
			}
			else if ( passaroRB.isKinematic && transform.position == GameManager.instance.posInicial.position )
			{

				lineBack.enabled = true;
				lineFront.enabled = true;
			}
		}
	}


	void DraggingWithTouch()
	{
		//print("dragging with touch");
		if ( touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved )
		{
			Vector3 tPos = Camera.main.ScreenToWorldPoint( new Vector3( touch.position.x, touch.position.y, 10 ) );
			catapultToBird = tPos - catapult.position;
			if ( catapultToBird.sqrMagnitude > 9.0f )
			{
				rayToMT.direction = catapultToBird;
				tPos = rayToMT.GetPoint( 3f );
			}
			transform.position = tPos;
		}

		if ( touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled )
		{
			print("ended");
			DesactivarDrag();
			picked = false;
		}
	}
	void DraggingWithMouse()
	{
		//print("dragging with mouse");

		if ( passaroRB.isKinematic )
		{
			Vector3 mouseWp = Camera.main.ScreenToWorldPoint( Input.mousePosition );
			mouseWp.z = 0f;

			catapultToBird = mouseWp - catapult.position;
			if ( catapultToBird.sqrMagnitude > 9.0f )
			{
				rayToMT.direction = catapultToBird;
				mouseWp = rayToMT.GetPoint( 3f );
			}

			transform.position = mouseWp;
		}


	}
	void ActivarDrag()
	{
		if (GameManager.instance.pausado == false )
		{
			clicked = true;
			rastro.enabled = false;
			EstouPronto = true;
		}
	}
	void DesactivarDrag()
	{

		passaroRB.isKinematic = false;
		clicked = false;
		if ( rastro != null )
		{
			rastro.enabled = true;
			if(GameManager.instance.rastroPrevio == null )
			{
				GameManager.instance.rastroPrevio = rastro;
			}
			else
			{
				GameManager.instance.rastroPrevio.GetComponent<MataRastro>().MataRastroPassaro();
				GameManager.instance.rastroPrevio = rastro;
			}
			
		}
		GameManager.instance.passaroLancado = true;
		audioPassaro.Play();
	}
	void OnMouseDown()
	{
		if ( transform.position == GameManager.instance.posInicial.position )
		{
			ActivarDrag();
		}

	}
	void OnMouseUp()
	{
		if ( EstouPronto )
		{
			DesactivarDrag();
		}

	}

	//----------------Morte Passaro------------------------
	void MataPassaro()
	{
		if ( passaroRB.velocity.magnitude == 0 && passaroRB.IsSleeping() )
		{
			EstaMorrendo = true;
			StartCoroutine( TempoMorte() );
			//print( "inicia morte" );
		}
	}
	IEnumerator TempoMorte()
	{
		yield return new WaitForSeconds( TempoParaMorrer );
		Instantiate( bomb, new Vector3( transform.position.x, transform.position.y, -0.5f ), Quaternion.identity );
		EstouPronto = false;
		Instantiate( audioMortePassaro, new Vector3( transform.position.x, transform.position.y, -0.5f ), Quaternion.identity );

		Destroy( gameObject );
		GameManager.instance.passarosUsados++;
		GameManager.instance.passaroCarregado = false;
		GameManager.instance.passaroLancado = false;
		EstaMorrendo = false;
		//print( "Morreu" );
	}
	//-----------------------------------------------------
	private void OnTriggerEnter2D( Collider2D collision )
	{
		if ( collision.gameObject.CompareTag( "moeda" ) )
		{
			GameManager.instance.moedas += 50;
			UIManager.instance.moedas.text = GameManager.instance.moedas.ToString();
			Destroy(collision.gameObject);
		}
	}
}
