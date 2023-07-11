using UnityEngine;


public class AudioManager : MonoBehaviour
{
	public static AudioManager instance;
	public AudioSource _audioAmbient, _audioFX;
	public bool somActivo = true;

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
	public void PlayEffect(AudioClip clip)
	{
		_audioFX.PlayOneShot(clip);
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
	public bool ToggleAudio()
	{
		AudioListener.pause = !AudioListener.pause;
		return AudioListener.pause;
	}
}
