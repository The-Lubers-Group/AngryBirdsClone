using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PainelWin : Painel
{
	private Button[] buttons;
	public static PainelWin instance;
	public EstrelasAnimController estrelasAnim;
	
    // Start is called before the first frame update
    public override void Awake()
    {
		base.Awake();
		instance = this;
        buttons = GetComponentsInChildren<Button>();
		estrelasAnim = GetComponent<EstrelasAnimController>();
    }
	public void ActivateButtons()
	{
		foreach (Button button in buttons) 
		{
			if (button != null)
			{
				button.interactable = true;
			}
		}
	}
	public void DeactivateButtons()
	{
		foreach (Button button in buttons) 
		{
			if (button != null)
			{
				button.interactable = false;
			}
		}
	}
	public override void OnFinishOpen(int stars)
	{
		estrelasAnim.startStars(stars);
	}
	public void NextLevel()
	{
		GameManager.instance.StopAllCoroutines();
		if(OndeEstou.instance.faseN == "Level2_Mestra1" || OndeEstou.instance.faseN == "Level4_Mestra2" )
		{
			SceneManager.LoadScene(SceneManagement.menuFases);
		}
		else
		{	
			SceneManager.LoadScene(OndeEstou.instance.fase+1);
		}	
	}
	
}
