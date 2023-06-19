using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Position : MonoBehaviour
{
    public TextMeshProUGUI txt;
    public Touch toque;
    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            toque = Input.GetTouch(0);
            if(toque.phase == TouchPhase.Began)
            {
                if(toque.position.x > (Screen.width / 2))
                {
                    txt.text = "Direita";
                }
                else
                {
                    txt.text = "Esquerda";
                }
            }
        }
    }
}
