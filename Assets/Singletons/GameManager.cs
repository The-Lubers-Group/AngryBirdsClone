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
	public Transform CameraE, CameraD;
	public Rigidbody2D[] objetosDaCena;

	public int qtdPassaros;
	public int numPorcosCena;
	public int destrutiveisEmCena;
	public int passarosUsados = 0;
	public string passaroName;
	public Passaro passaroAtual;
	//Control 
	public bool pausado = false;
	public bool jogoComecou;
	public bool zeroMovimento;
	public bool continuarProcurando;
	public bool passaroCarregado;
	public bool passaroInDragging = false;
	private bool acabaramPassaros = false;
	//Objetos Control
	public bool mira = false;
	
	//UI 
	private int _score;
	public int Score {
		set
		{
			_score += value;
			ScoreManager.instance.AtualizarScore();
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
	
	//Som
	public AudioClip somLose;
	public AudioClip somWin;

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
	// dois fun��es que controlam o que vai acontecer quando a scena seja carregada ou descarregada
	private void OnEnable()
	{
		SceneManager.sceneLoaded += Carrega;
		SceneManager.sceneUnloaded += Descarregar;
	}

	//Verifica��o das condi�oes de ganhar ou perder, e recarregar passaro caso ainda tenha oportunidades.
	
	private void Update()
	{
		bool ganhou = numPorcosCena <= 0 && passaros.Length > 0;
		bool perdeu = numPorcosCena > 0 && acabaramPassaros;
		if(ganhou && continuarProcurando)
		{
			//a fun��o ZeroMove � executada quando houve uma vitoria mas ainda pode ter pontua��o para ganhar porque ela comprova se ainda h� algum movimento em nos corpos rigidos (rigidbody2d)
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
		Time.timeScale = 1.0f;
		pausado = false;
		jogoComecou = true;
		acabaramPassaros = false;
		passaroCarregado = false;
		passarosUsados = 0;
		menuLoseActive = false;
		menuWinActive = false;
		passaroInDragging = false;
		_score = 0;
		BestScore = ScoreManager.instance.BestScoreLoad(OndeEstou.instance.faseN);
		moedas =  CoinManager.instance.LoadDados();
		ScoreManager.instance.AtualizarScore();
	}
	private void Carrega( Scene cena, LoadSceneMode mode )
	{

		if ( cena.name.StartsWith("Level") )
		{	
			objetosDaCena = GameObject.FindObjectsOfType<Rigidbody2D>();

			posInicial = GameObject.FindWithTag( Constants.POSINICIAL_TAG ).GetComponent<Transform>();
			CameraD = GameObject.FindWithTag( Constants.CAMERADIR_TAG).GetComponent<Transform>();
			CameraE = GameObject.FindWithTag( Constants.CAMERAESQ_TAG ).GetComponent<Transform>();
			passaros = GameObject.FindGameObjectsWithTag( Constants.PLAYER_TAG );
			numPorcosCena = GameObject.FindGameObjectsWithTag( Constants.INIMIGOPORCO_TAG ).Length;
			destrutiveisEmCena = GameObject.FindGameObjectsWithTag( Constants.DESTRUTIVEL_TAG ).Length;

			qtdPassaros = passaros.Length;
			SelectionSortByPositionX(passaros, qtdPassaros);

			ScoreMax = ( destrutiveisEmCena * 1000 ) + ( numPorcosCena * 5000 );
			//Score Max Com Passaros (Ainda n�o adicionado)
			//ScoreMax = ( destrutiveisEmCena * 1000 ) + ( numPorcosCena * 5000 ) + ( ( qtdPassaros - 1 ) * 5000 );
			AudioManager.instance.PlayAmbient();
			StartGame();
		}
	}
	//Ordena os passaros pela sua posi��o em x de direita para esquerda
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
	//comproba que n�o tem passaro j� carregado e nem passaro lancado que ainda pode estar fazendo dano.
	private void RecargarPassaro()
	{
		if ( !passaroCarregado )
		{
			if ( passarosUsados < passaros.Length )
			{
				int passaroNumero = passarosUsados;
				if ( passaros[ passaroNumero ].transform.position != posInicial.position && !passaroCarregado )
				{
					passaroName = passaros[ passaroNumero ].name;
					passaros[ passaroNumero ].transform.position = posInicial.position;
					passaroAtual = passaros[passaroNumero].GetComponent<Passaro>();
					Catapult.instance.LineUpdate();
					Catapult.instance.LineRendererEnabled();
					passaroCarregado = true;
				}
			}
			else
			{
				acabaramPassaros = true;
				return;
			}
		}
	}

	//Processo de perder
	private void GameOver()
	{
		jogoComecou = false;
		menuLoseActive = true;
		PainelLose.instance.openMenu(2.0f);
		AudioManager.instance.PlayEffect(somLose);
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
		
		//Anima��es
		estrelasObtidas = CalcularEstrelasComPontuacao();
		PainelWin.instance.openMenu(2.0f,estrelasObtidas);

		//Som
		AudioManager.instance.PlayEffect(somWin);

		//Saves
		SaveLevelStars();
		CoinManager.instance.SalvarDados(CoinManager.instance.GetMoneyTemp());
		ScoreManager.instance.BestScoreSave(OndeEstou.instance.faseN, _score);

		//Liberar ProximaFase
		int levelProximo = OndeEstou.instance.fase + 1;
		ZPlayerPrefs.SetInt("Level" + levelProximo + "_" + OndeEstou.instance.faseMestra, 1);
	}
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
	// Este � um metodo geral para programar o que fazer caso seja descarregada a cena
	private void Descarregar(Scene current)
	{
		StopAllCoroutines();
		//Asegura que todas as anima��es que estejam rodando sejam destruidas
		DOTween.KillAll();
	}
	void OnDisable()
    {
        SceneManager.sceneLoaded -= Carrega;
		SceneManager.sceneUnloaded -= Descarregar;
    }
}
