using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnConfMainMenu : MonoBehaviour
{
    public bool liga = false;
    public Animator animaConf;
    private void Awake()
    {
        animaConf = GetComponentInChildren<Animator>();
		GetComponent<Button>().onClick.AddListener(ClickBTN);
    }

    public void ClickBTN()
    {
        liga = !liga;
        if (liga)
        {
            animaConf.Play("MoveUIConf");
        }
        else
        {
            animaConf.Play("MoveUIConfInverse");
        }
    }
	
}
