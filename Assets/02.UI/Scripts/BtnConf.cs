using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnConf : MonoBehaviour
{
    public static BtnConf instance;
    public bool liga = false;
    public Animator animaConf;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
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
