using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OndeEstou : MonoBehaviour
{
	public static OndeEstou instance;
	public int fase = -1;
	public string faseN;
	public string faseMestra;
	private void Awake()
	{
		if(instance == null )
		{
			instance = this;
			DontDestroyOnLoad( this.gameObject );
		}
		else
		{
			Destroy(this.gameObject);
		}
		SceneManager.sceneLoaded += VerificaFase;
	}

	void VerificaFase(Scene cena, LoadSceneMode modo) 
	{
		fase = SceneManager.GetActiveScene().buildIndex;
		faseN = SceneManager.GetActiveScene().name;
	}
	public void Mestra(string nome )
	{
		faseMestra = nome;
		SceneManager.LoadScene(faseMestra);
	} 
	[ContextMenu( "Reset All  playerprefs" )]
	void ResetPlayerPref()
	{
		ZPlayerPrefs.DeleteAll();
	}
}
