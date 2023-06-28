using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
	public static LevelManager instance;
	private int levelsMestre1 = 0, levelsMestre2 = 2;

	void Awake()
	{
		if ( instance == null )
		{
			instance = this;
		}
		else
		{
			Destroy( this.gameObject );
		}

	}
	void Start()
	{
		ZPlayerPrefs.SetInt( "Level1", 1 );
		ZPlayerPrefs.SetInt( "Level2", 1 );
		ListaAdd();
	}

	[System.Serializable]
	public class Level
	{
		public string levelText;
		public bool habilitado;
		public int desbloqueado;
		public bool txtAtivo;
		public string levelReal;
	}
	public GameObject botao;
	public Transform localBtn;
	public List<Level> levelList;

	void ListaAdd()
	{
		foreach ( Level level in levelList )
		{
			GameObject btnNovo = Instantiate( botao ) as GameObject;
			BtnLevel infoBtnLevel = btnNovo.GetComponent<BtnLevel>();
			infoBtnLevel.levelTxt.text = level.levelText;
			infoBtnLevel.realLevel = level.levelReal;
			string keyLevel = "Level" + infoBtnLevel.realLevel + "_" + OndeEstou.instance.faseMestra;
			if ( ZPlayerPrefs.HasKey( keyLevel ) )
			{
				if ( ZPlayerPrefs.GetInt( keyLevel ) == 1 )
				{
					level.desbloqueado = 1;
					level.habilitado = true;
					level.txtAtivo = true;
				}
			}


			infoBtnLevel.desbloqueadoBTN = level.desbloqueado;
			//por padran os botoes estan sem texto habilidato
			infoBtnLevel.GetComponentInChildren<TextMeshProUGUI>().enabled = level.txtAtivo;
			//configurar um sprite no interactable component
			infoBtnLevel.GetComponent<Button>().interactable = level.habilitado;
			infoBtnLevel.GetComponent<Button>().onClick.AddListener( () => ClickLevel( keyLevel ) );

			string KeyEstrelas = "Level" + infoBtnLevel.realLevel + "_" + OndeEstou.instance.faseMestra + "estrelas";

			if ( ZPlayerPrefs.HasKey( KeyEstrelas ) )
			{
				if ( ZPlayerPrefs.GetInt( KeyEstrelas ) == 1 )
				{
					infoBtnLevel.estrela1.enabled = true;
				}
				else if ( ZPlayerPrefs.GetInt( KeyEstrelas ) == 2 )
				{
					infoBtnLevel.estrela1.enabled = true;
					infoBtnLevel.estrela2.enabled = true;
				}
				else if ( ZPlayerPrefs.GetInt( KeyEstrelas ) == 3 )
				{
					infoBtnLevel.estrela1.enabled = true;
					infoBtnLevel.estrela2.enabled = true;
					infoBtnLevel.estrela3.enabled = true;
				}
				else
				{
					infoBtnLevel.estrela1.enabled = false;
					infoBtnLevel.estrela2.enabled = false;
					infoBtnLevel.estrela3.enabled = false;
				}
			}

			if (OndeEstou.instance.faseMestra == "Mestra1")
			{
				levelsMestre1++;
				ZPlayerPrefs.SetInt("FasesNumMestra1", levelsMestre1);
			} 
			else if (OndeEstou.instance.faseMestra == "Mestra2")
			{
				levelsMestre2++;
				ZPlayerPrefs.SetInt("FasesNumMestra2", levelsMestre2);

			}
			btnNovo.transform.SetParent( localBtn, false );
		}
	}
	void ClickLevel( string level )
	{
		SceneManager.LoadScene( level );
	}
	// Start is called before the first frame update
	[ContextMenu( "Reset All  playerprefs" )]
	void ResetPlayerPref()
	{
		ZPlayerPrefs.DeleteAll();
	}
}
