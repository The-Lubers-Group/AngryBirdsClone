using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBtnMenu : MonoBehaviour
{
    public Animator btnAnim;
    private bool chave = true;
    
    public void EventoClickG()
    {
        chave = !chave;
        if(chave == false)
        {
            btnAnim.Play("BtnAnimMaisGames");
        }
        else
        {
            btnAnim.Play("InverseBtnAnimMaisGames");
        }
    }
}
