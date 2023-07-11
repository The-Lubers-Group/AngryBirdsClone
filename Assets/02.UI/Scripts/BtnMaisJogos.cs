using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnMaisJogos : MonoBehaviour
{
    public bool ativo = false;
    public Animator anima;
    private void Awake()
    {
        anima = GetComponentInChildren<Animator>();
		GetComponent<Button>().onClick.AddListener(onClick);
    }

    public void onClick()
    {
        ativo = !ativo;
        if (ativo)
        {
            anima.Play("BtnAnimMaisGames");
        }
        else
        {
            anima.Play("InverseBtnAnimMaisGames");
        }
    }
	
}
