using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
	public static AudioManager instance;
	public AudioSource _audioAmbient, _audioWinMenu, _audioLoseMenu;


    // Start is called before the first frame update
    void Awake()
    {
		if(instance == null )
		{
			instance = this;
			DontDestroyOnLoad( this.gameObject );
		}
		else
		{
			Destroy( this.gameObject );
		}
		
		_audioAmbient = GetComponent<AudioSource>();
	}
	private void OnEnable()
	{
		SceneManager.sceneLoaded += Carrega;
	}
	private void Carrega(Scene cena,  LoadSceneMode modo)
	{	
		Regex rx = new Regex(@"Level[0-9]{1}");

		if ( rx.Match( cena.name ).Success )
		{
			_audioWinMenu = GameObject.FindWithTag("UIMenuWin").GetComponent<AudioSource>();
			_audioLoseMenu = GameObject.FindWithTag("UIMenuLose").GetComponent<AudioSource>();
		}
	}
	private void OnDisable()
	{
		SceneManager.sceneLoaded -= Carrega;
	}
	private void Start()
	{
			PlayAmbient();
	}
	// Update is called once per frame
	public void PlayAmbient()
    {
		if ( !_audioAmbient.isPlaying )
		{
			_audioAmbient.Play();
		}
    }
	public void PlayAudioWinMenu()
	{
		if ( !_audioWinMenu.isPlaying )
		{
			_audioWinMenu.PlayOneShot(_audioWinMenu.clip);
		}
	}
	public void PlayAudioLoseMenu()
	{
		if ( !_audioLoseMenu.isPlaying )
		{
			
			_audioLoseMenu.PlayOneShot(_audioLoseMenu.clip);
		}
	}
	public void PauseAll()
	{
		AudioListener.pause = true;
		//_audioAmbient.Pause();
	}
	public void PlayAll()
	{
		AudioListener.pause = false;
	}
}
