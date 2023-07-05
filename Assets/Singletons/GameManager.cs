using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface IDamageable
{
	void Damage(Rigidbody2D rb);
	void Danificar();
	void ProcessarMorte();
}
public class GameManager : MonoBehaviour
{
	public static GameManager instance;


	public GameObject[] passaros;
	public TrailRenderer rastroPrevio;
	public Transform posInicial;
	public Transform CameraE, objD;
	public Rigidbody2D[] objetosDaCena;

	public int qtdPassaros;
	public int numPorcosCena;
	public int destrutiveisEmCena;
	public int passarosUsados = 0;
	public string passaroName;

	//Control 
	public bool pausado = false;
	public bool jogoComecou;
	public bool zeroMovimento;
	public bool continuarProcurando;
	public bool passaroCarregado;
	public bool passaroLancado = false;
	public bool passaroInDragging = false;

	//Objetos Control
	public bool mira = false;
	
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
	// dois funções que controlam o que vai acontecer quando a scena seja carregada ou descarregada
	private void OnEnable()
	{
		SceneManager.sceneLoaded += Carrega;
		SceneManager.sceneUnloaded += Descarregar;
	}

	//Verificação das condiçoes de ganhar ou perder, e recarregar passaro caso ainda tenha oportunidades.
	
	private void Update()
	{
		bool ganhou = numPorcosCena <= 0 && passaros.Length > 0;
		bool perdeu = numPorcosCena > 0 && passarosUsados == qtdPassaros ;
		if(ganhou && continuarProcurando)
		{
			//a função ZeroMove é executada quando houve uma vitoria mas ainda pode ter pontuação para ganhar porque ela comprova se ainda há algum movimento em nos corpos rigidos (rigidbody2d)
			zeroMovimento = ZeroMove();
		}
		if (perdeu && !menuWinActive && !menuLoseActive)
		{
			GameOver();
		}
		else if ( ganhou && !menuWinActive && !menuLoseActive)
		{
			StartCoroutine(WinGame());
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
		passaroInDragging = false;
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
			objetosDaCena = GameObject.FindObjectsOfType<Rigidbody2D>();

			posInicial = GameObject.FindWithTag( "posInicial" ).GetComponent<Transform>();
			objD = GameObject.FindWithTag( "CameraDir" ).GetComponent<Transform>();
			CameraE = GameObject.FindWithTag( "CameraEsq" ).GetComponent<Transform>();
			passaros = GameObject.FindGameObjectsWithTag( "Player" );
			numPorcosCena = GameObject.FindGameObjectsWithTag( "InimigoPorco" ).Length;
			destrutiveisEmCena = GameObject.FindGameObjectsWithTag( "Destrutivel" ).Length;

			qtdPassaros = passaros.Length;
			SelectionSortByPositionX(passaros, qtdPassaros);

			ScoreMax = ( destrutiveisEmCena * 1000 ) + ( numPorcosCena * 5000 );
			//Score Max Com Passaros (Ainda não adicionado)
			//ScoreMax = ( destrutiveisEmCena * 1000 ) + ( numPorcosCena * 5000 ) + ( ( qtdPassaros - 1 ) * 5000 );
			AudioManager.instance.PlayAmbient();
			StartGame();
		}
	}
	//Ordena os passaros pela sua posição em x de direita para esquerda
	void SelectionSortByPositionX(GameObject []arr, int n) 
	{
		for(int i = 0; i < n - 1; ++i) 
		{
			int min_index = i; 
			for(int j = i + 1; j < n; ++j) 
			{
				float x1 = arr[j].transform.position.x;
                float x2 = arr[min_index].transform.position.x;
				if(x1 > x2)
				{
					min_index = j;
				}
			}
			GameObject temp = arr[i];
			arr[i] = arr[min_index];
			arr[min_index] = temp;
		}
	}
	

	private bool ZeroMove()
	{
		foreach ( Rigidbody2D item in objetosDaCena )
		{
			if(item != null)
			{
				if(!item.IsSleeping())
			{
				//print(item.gameObject.name);
				return false;
			}
			}
		}
		//print("no hay movimento");
		return true;
	}
	//comproba que não tem passaro já carregado e nem passaro lancado que ainda pode estar fazendo dano.
	private void RecargarPassaro()
	{
		if ( !passaroCarregado && !passaroLancado )
		{
			if ( passarosUsados < passaros.Length )
			{
				int passaroNumero = passarosUsados;
				if ( passaros[ passaroNumero ].transform.position != posInicial.position && !passaroCarregado )
				{
					passaroName = passaros[ passaroNumero ].name;
					passaros[ passaroNumero ].transform.position = posInicial.position;
					Drag passaroAtual = passaros[passaroNumero].GetComponent<Drag>();
					passaroAtual.LineUpdate();
					passaroAtual.lineBack.enabled = true;
					passaroAtual.lineFront.enabled = true;
					passaroCarregado = true;
				}
			}
			else
			{
				//print("acabaram os passaros");
				return;
			}
		}
	}

	//Processo de perder
	private void GameOver()
	{
		jogoComecou = false;
		menuLoseActive = true;
		StartCoroutine(DelayAnim(UIManager.instance.painelGameOver, "Anim_MenuLose", 2f, AbrirMenu));
		AudioManager.instance.PlayAudioLoseMenu();
	}
	//Processo de Ganhar
	IEnumerator WinGame()
	{
		//Status do Jogo
		jogoComecou = false;
		menuWinActive = true;
		continuarProcurando = true;
		yield return new WaitUntil(() => zeroMovimento);
		continuarProcurando = false;
		
		//Animações
		estrelasObtidas = CalcularEstrelasComPontuacao();
		EstrelasAnimController.instance.Qtd = estrelasObtidas;
		EstrelasAnimController.instance.IniciarCoroutine();
		StartCoroutine(DelayAnim(UIManager.instance.painelWin, "Anim_MenuWin", 2f, AbrirMenu));

		//Som
		AudioManager.instance.PlayAudioWinMenu();

		//Saves
		SaveLevelStars();
		CoinManager.instance.SalvarDados(moedas);
		PointManager.instance.BestScoreSave(OndeEstou.instance.faseN, _score);

		//Liberar ProximaFase
		int levelProximo = OndeEstou.instance.fase + 1;
		ZPlayerPrefs.SetInt("Level" + levelProximo + "_" + OndeEstou.instance.faseMestra, 1);
	}
	// ------------------------- Delay para as animações----------------------------
	public delegate void Del(Animator anim, string nomeAnim);
	public static void AbrirMenu(Animator anim, string nomeAnim )
	{
		anim.Play(nomeAnim);
		UIManager.instance.fundoPreto.enabled = true;
		UIManager.instance.LineRendererDisabled();
	}
	IEnumerator DelayAnim(Animator anim, string nome, float seg, Del callback)
	{
		yield return new WaitForSeconds(seg);
		callback(anim, nome);
		yield return null;
	}// ------------------------- Fim Delay para as animações----------------------------

	//Calculo com base a pocentagem obtida do total score que pode ser conseguido
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
	//Guarda a quantidade de estrelas obtidas no nivel 
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
	// Este é um metodo geral para programar o que fazer caso seja descarregada a cena
	private void Descarregar(Scene current)
	{
		//Asegura que todas as animações que estejam rodando sejam destruidas
		DOTween.KillAll();
	}
	void OnDisable()
    {
        SceneManager.sceneLoaded -= Carrega;
		SceneManager.sceneUnloaded -= Descarregar;
    }
}
