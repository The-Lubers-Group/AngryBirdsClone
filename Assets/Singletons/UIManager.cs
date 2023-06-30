using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
	public static UIManager instance;

	// Tela Inicial UI
	public Button playBtnInicial;
	public TextMeshProUGUI txtCarregando;
	
	// Menu Principal
	public Animator animConfMainMenu;
	public Animator animMaisJogosMainMenu;
	public Button btnMaisJogosMainMenu;
	public Button btnLojaMainMenu;
	public Button btnConfMainMenu;
	public Button btnPlayMainMenu;
	public Button btnCloseCreditsMainMenu;
	public Button btnInfoMainMenu;
	public bool somActivo = true;
	//public Button btnSoundMainMenu;
	public MoveTxtInfo infoTextMainMenu;
	public bool ligaAnimBtnConfMainMenu = false;
	public bool ligaAnimBtnMainJogoMainMenu = false;

	//Menu Fases
	public Button btnBackFases;
	
	//Menu Level
	public Button btnBackLevel;

	// Level UI
	public Animator painelGameOver, painelWin, painelPause;
	public Button winBtnMenu, winBtnNovamente, winBtnProximo;
	[SerializeField] private Button loseBtnMenu, loseBtnNovamente;
	[SerializeField] private Button pauseBtn, pauseBtnPlay, pauseBtnNovamente, pauseBtnMenu, pauseBtnLoja;
	public TextMeshProUGUI score, bestScore, moedas;
	public Image fundoPreto;
	public AudioSource winSom;
	private LineRenderer frontLR;
	private LineRenderer backLR;
	public DragCamera2D camDrag;

	//Loja
	public Button btnBackLoja;
	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(this.gameObject);
		}
		else
		{
			Destroy(this.gameObject);
		}
	}
	private void OnEnable()
	{
		SceneManager.sceneLoaded += Carrega;
	}
	private void OnDisable()
    {
        SceneManager.sceneLoaded -= Carrega;

    }

	void Carrega(Scene cena, LoadSceneMode modo )
	{

		if (cena.name == "Inicial")
		{
			playBtnInicial = GameObject.FindGameObjectWithTag("UIBtnInicial").GetComponent<Button>();
			playBtnInicial.onClick.AddListener(GoToMainMenuAsync);
		}
		else if(cena.name == "Loja" )
		{
			btnBackLoja = GameObject.FindWithTag("UIBtnBackLoja").GetComponent<Button>();
			btnBackLoja.onClick.AddListener(GoToMainMenu);
		}
		else if(cena.name == "MenuFases" )
		{
			btnBackFases =  GameObject.FindWithTag("UIBtnBackFases").GetComponent<Button>();
			btnBackFases.onClick.AddListener(GoToMainMenu);
		}
		else if ( cena.name.StartsWith( "Mestra" ) )
		{
			btnBackLevel = GameObject.FindWithTag("UIBtnBackLevel").GetComponent<Button>();
			btnBackLevel.onClick.AddListener(GoToMenuFases);

		}
		else if ( cena.name == "MenuPrincipal")
		{
			animConfMainMenu = GameObject.FindWithTag("UIAnimConfMainMenu").GetComponent<Animator>();
			animMaisJogosMainMenu = GameObject.FindWithTag("UIAnimMaisJogosMainMenu").GetComponent<Animator>();

			btnConfMainMenu = GameObject.FindWithTag("UIBtnConfMainMenu").GetComponent<Button>();
			btnMaisJogosMainMenu = GameObject.FindWithTag("UIAnimMaisJogosMainMenu").GetComponent<Button>();
			btnLojaMainMenu = GameObject.FindWithTag("UIBtnLojaMainMenu").GetComponent<Button>();
			btnPlayMainMenu = GameObject.FindWithTag("UIBtnPlayMainMenu").GetComponent<Button>();
			//btnSoundMainMenu = GameObject.FindWithTag("UIBtnSoundMainMenu").GetComponent<Button>();
			btnInfoMainMenu = GameObject.FindWithTag("UIBtnInfoMainMenu").GetComponent<Button>();
			infoTextMainMenu = GameObject.FindWithTag("infoTxt").GetComponent<MoveTxtInfo>();

			btnConfMainMenu.onClick.AddListener(PlayAnimBtnConf);
			btnMaisJogosMainMenu.onClick.AddListener(PlayAnimBtnMaisJogos);
			btnLojaMainMenu.onClick.AddListener(GoToLoja);
			btnPlayMainMenu.onClick.AddListener(GoToMenuFases);
			btnInfoMainMenu.onClick.AddListener(infoTextMainMenu.LiberaMove);
			ligaAnimBtnConfMainMenu = false;
			ligaAnimBtnMainJogoMainMenu = false;
			//btnSoundMainMenu.onClick.AddListener(btnSoundMainMenu.GetComponent<btnAudioToggleIcon>().btn);
		}
		//Level
		else if ( cena.name.StartsWith("Level") )
		{
			//line renderers
			frontLR = GameObject.FindWithTag("CatapultLF").GetComponent<LineRenderer>();
			backLR = GameObject.FindWithTag("CatapultLB").GetComponent<LineRenderer>();

			//Painel
			painelGameOver = GameObject.Find("MenuLose").GetComponent<Animator>();
			painelWin = GameObject.Find("MenuWin").GetComponent<Animator>();
			painelPause = GameObject.Find("MenuPause").GetComponent<Animator>();
			fundoPreto = GameObject.FindWithTag("UIFundoPreto").GetComponent<Image>();

			//Btn Win
			winBtnMenu = GameObject.Find("WinBtnMenu").GetComponent<Button>();
			winBtnNovamente = GameObject.Find("WinBtnAgain").GetComponent<Button>();
			winBtnProximo = GameObject.Find("WinBtnNext").GetComponent<Button>();

			//Btn Lose
			loseBtnMenu =   GameObject.Find("LoseBtnMenu").GetComponent<Button>();
			loseBtnNovamente =   GameObject.Find("LoseBtnAgain").GetComponent<Button>();

			//Btn Pause
			pauseBtn =  GameObject.Find("BtnPause").GetComponent<Button>();
			pauseBtnPlay =  GameObject.Find("PauseBtnPlay").GetComponent<Button>();
			pauseBtnNovamente =  GameObject.Find("PauseBtnAgain").GetComponent<Button>();
			pauseBtnMenu =  GameObject.Find("PauseBtnMenu").GetComponent<Button>();
			pauseBtnLoja =  GameObject.Find("PauseBtnStore").GetComponent<Button>();
			camDrag = Camera.main.GetComponent<DragCamera2D>();

			//Audio
			winSom = painelWin.GetComponent<AudioSource>();

			//Score
			score = GameObject.FindGameObjectWithTag("UIScore").GetComponent<TextMeshProUGUI>();
			bestScore = GameObject.FindGameObjectWithTag("UIBestScore").GetComponent<TextMeshProUGUI>();

			//Moedas
			moedas = GameObject.FindGameObjectWithTag("UIMoeda").GetComponent<TextMeshProUGUI>();

			//Eventos Menu Pause
			pauseBtn.onClick.AddListener(Pausar);
			pauseBtnPlay.onClick.AddListener(Continuar);
			pauseBtnNovamente.onClick.AddListener(PlayAgain);
			pauseBtnLoja.onClick.AddListener(GoToLoja);
			pauseBtnMenu.onClick.AddListener(GoToMenuFases);

			//Eventos Menu Lose
			loseBtnMenu.onClick.AddListener(GoToMenuFases);
			loseBtnNovamente.onClick.AddListener(PlayAgain);
			
			//Eventos Menu Win
			winBtnMenu.onClick.AddListener(GoToMenuFases);
			winBtnNovamente.onClick.AddListener(PlayAgain);
			winBtnProximo.onClick.AddListener(NextLevel);

			
		}
	}
	private void GoToMainMenuAsync()
	{
		StartCoroutine(LoadGameProg());
	}
	
	IEnumerator LoadGameProg()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync("MenuPrincipal");
        while (!async.isDone)
        {
            txtCarregando.enabled = true;
            yield return null;
        }
    }
	private void NextLevel()
	{
		if(OndeEstou.instance.faseN == "Level2_Mestra1" || OndeEstou.instance.faseN == "Level4_Mestra2" )
		{
			GameManager.instance.StopAllCoroutines();
			SceneManager.LoadScene("MenuFases");
		}
		else
		{
			GameManager.instance.StopAllCoroutines();
			SceneManager.LoadScene(OndeEstou.instance.fase+1);
		}	
	}
	private void Pausar()
	{
		GameManager.instance.pausado = true;
		Time.timeScale = 0;
		fundoPreto.enabled = true;
		camDrag.enabled = false;
		painelPause.Play("Anim_PainelPause");
		LineRendererDisabled();
	}
	public void LineRendererDisabled()
	{	
		frontLR.enabled = false;
		backLR.enabled = false;
	}
	public void LineRendererEnabled()
	{
		if(!GameManager.instance.passaroLancado)
		{
			frontLR.enabled = true;
			backLR.enabled = true;
		} 
		
	}
	private void Continuar()
	{
		GameManager.instance.pausado = false;
		camDrag.enabled = true;
		Time.timeScale = 1;
		fundoPreto.enabled = false;
		painelPause.Play("Anim_PainelPauseInverse");
		LineRendererEnabled();
	}
	private void PlayAgain()
	{
		GameManager.instance.StopAllCoroutines();
		GameManager.instance.pausado = false;
		Time.timeScale = 1;
		SceneManager.LoadScene(OndeEstou.instance.fase);
		
	}
	private void GoToMenuFases()
	{
		if ( SceneManager.GetActiveScene().name.StartsWith( "Level" ) )
		{
			GameManager.instance.StopAllCoroutines();
			
		}
		SceneManager.LoadScene("MenuFases");

		if(Time.timeScale != 1 )
		{
			Time.timeScale = 1;
			GameManager.instance.pausado = false;
		}
	}
	private void GoToLoja()
	{
		if ( SceneManager.GetActiveScene().name.StartsWith( "Level" ) )
		{
			GameManager.instance.StopAllCoroutines();
			
		}
		SceneManager.LoadScene("Loja");
		if(Time.timeScale != 1 )
		{
			Time.timeScale = 1;
			GameManager.instance.pausado = false;
		}
	}
	private void GoToMainMenu()
	{
		SceneManager.LoadScene("MenuPrincipal");
	}
	public void AtualizarScore()
	{
		int novoScore = GameManager.instance.Score;
		score.text = novoScore.ToString();
		bestScore.text = GameManager.instance.BestScore.ToString();
		if (novoScore > int.Parse(bestScore.text))
		{
			bestScore.text = score.text;
		}
	}
	public void BaixaGame()
    {
        Application.OpenURL("https://play.google.com");
    }

	public void PlayAnimBtnConf()
    {
        ligaAnimBtnConfMainMenu = !ligaAnimBtnConfMainMenu;
        if (ligaAnimBtnConfMainMenu)
        {
            animConfMainMenu.Play("MoveUIConf");
        }
        else
        {
            animConfMainMenu.Play("MoveUIConfInverse");
        }
    }
	private void PlayAnimBtnMaisJogos()
	{
		ligaAnimBtnMainJogoMainMenu = !ligaAnimBtnMainJogoMainMenu;
        if(ligaAnimBtnMainJogoMainMenu)
        {
            animMaisJogosMainMenu.Play("BtnAnimMaisGames");
        }
        else
        {
            animMaisJogosMainMenu.Play("InverseBtnAnimMaisGames");
        }
    }
	
}
