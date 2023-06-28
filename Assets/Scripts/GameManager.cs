using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	public int QtdPassaros;
	public bool passaroCarregado;
	public string nomePassaro;
	public int passarosUsados = 0;
	public bool passaroLancado = false;
	public bool pausado = false;
	public bool jogoComecou;

	public GameObject[] passaro;
	public TrailRenderer rastroPrevio;
	public Transform posInicial;
	public Transform objE, objD;

	public int numPorcosCena;
	public int DestrutiveisEmCena;

	//UI 
	private int _score;
	public int Score {
		set
		{
			_score += value;
			UIManager.instance.AtualizarScore();
		}
		get {	return _score; }
	}

	public int ScoreMax;
	public int BestScore;
	public int moedas;
	
	//Menus
	public int estrelasObtidas;
	public bool menuWinActive;
	public bool menuLoseActive;
	
	
	private void Awake()
	{
		if ( instance == null )
		{
			instance = this;
			DontDestroyOnLoad( this.gameObject );
		}
		else
		{
			Destroy( this.gameObject );
		}
		
	}
	private void OnEnable()
	{
		SceneManager.sceneLoaded += Carrega;
		SceneManager.sceneUnloaded += Descarregar;
	}

	private void Update()
	{
		bool ganhou = numPorcosCena <= 0 && passaro.Length > 0;
		bool perdeu = numPorcosCena > 0 && passarosUsados == QtdPassaros ;
		
		if (perdeu && !menuWinActive && !menuLoseActive)
		{
			GameOver();
		}
		else if ( ganhou && !menuWinActive && !menuLoseActive)
		{
			WinGame();
		}
		else
		{
			RecargarPassaro();
		}
	}
	
	private	void StartGame()
	{
		jogoComecou = true;
		passaroCarregado = false;
		passarosUsados = 0;
		passaroLancado = false;
		menuLoseActive = false;
		menuWinActive = false;
		_score = 0;
		BestScore = PointManager.instance.BestScoreLoad(OndeEstou.instance.faseN);
		moedas =  CoinManager.instance.LoadDados();
		UIManager.instance.AtualizarScore();
		UIManager.instance.moedas.text = moedas.ToString();
	}
	private void Carrega( Scene cena, LoadSceneMode mode )
	{

		if ( cena.name.StartsWith("Level") )
		{
			posInicial = GameObject.FindWithTag( "posInicial" ).GetComponent<Transform>();
			objD = GameObject.FindWithTag( "CameraDir" ).GetComponent<Transform>();
			objE = GameObject.FindWithTag( "CameraEsq" ).GetComponent<Transform>();

			QtdPassaros = GameObject.FindGameObjectsWithTag( "Player" ).Length;
			numPorcosCena = GameObject.FindGameObjectsWithTag( "InimigoPorco" ).Length;
			DestrutiveisEmCena = GameObject.FindGameObjectsWithTag( "Destrutivel" ).Length;

			passaro = new GameObject[ QtdPassaros ];
			for ( int i = 0; i < QtdPassaros; i++ )
			{
				passaro[ i ] = GameObject.Find( "Bird" + i );
			}
			ScoreMax = ( DestrutiveisEmCena * 1000 ) + ( numPorcosCena * 5000 ) + ( ( QtdPassaros - 1 ) * 5000 );
			AudioManager.instance.PlayAmbient();
			//print("carregou");
			StartGame();
		}
	}

	private void RecargarPassaro()
	{
		if ( !passaroCarregado && !passaroLancado )
		{
			if ( passarosUsados < passaro.Length )
			{
				int passaroNumero = passarosUsados;
				//print(passaroNumero);
				if ( passaro[ passaroNumero ].transform.position != posInicial.position && !passaroCarregado )
				{
					nomePassaro = passaro[ passaroNumero ].name;
					print( nomePassaro );
					passaro[ passaroNumero ].transform.position = posInicial.position;
					passaroCarregado = true;
					print("colocado passaro");
				}
			}
			else
			{
				print("acabaram os passaros");
				return;
			}
		}
	}

	private void GameOver()
	{
		jogoComecou = false;
		menuLoseActive = true;
		UIManager.instance.painelGameOver.Play( "Anim_MenuLose" );
		AudioManager.instance.PlayAudioLoseMenu();
		print("menu mandado");
	}
	private void WinGame()
	{
		//Status do Jogo
		jogoComecou = false;
		menuWinActive = true;

		//Animações
		estrelasObtidas = CalcularEstrelasComPontuacao();
		EstrelasAnimController.instance.Qtd = estrelasObtidas;
		EstrelasAnimController.instance.IniciarCoroutine();
		UIManager.instance.painelWin.Play( "Anim_MenuWin" );
		AudioManager.instance.PlayAudioWinMenu();

		//Saves
		SaveLevelStars();
		CoinManager.instance.SalvarDados(moedas);
		PointManager.instance.BestScoreSave(OndeEstou.instance.faseN, _score);

		//Liberar ProximaFase
		int levelProximo = OndeEstou.instance.fase + 1;
		ZPlayerPrefs.SetInt("Level" + levelProximo + "_" + OndeEstou.instance.faseMestra, 1);
	}

	private int CalcularEstrelasComPontuacao()
	{
		float porcentagemObtido = ( _score * 100 ) / ScoreMax;

		if ( porcentagemObtido >= 75f )
		{
			return 3;
		}
		else if ( porcentagemObtido >= 35 )
		{
			return 2;
		}
		else
		{
			return 1;
		}
	}
	private void SaveLevelStars()
	{
		string key = OndeEstou.instance.faseN + "estrelas";
		
		if ( ZPlayerPrefs.HasKey(key ) )
		{
			if ( ZPlayerPrefs.GetInt( key ) < estrelasObtidas )
			{
				ZPlayerPrefs.SetInt( key, estrelasObtidas );
			} 
		}
		else
		{
			ZPlayerPrefs.SetInt( key, estrelasObtidas );
		}
	}
	private void Descarregar(Scene current)
	{
		DOTween.KillAll();
	}
	void OnDisable()
    {
        SceneManager.sceneLoaded -= Carrega;
		SceneManager.sceneUnloaded -= Descarregar;
    }
}
