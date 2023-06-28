using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.UI;

public class EstrelaAnim : MonoBehaviour
{
    public float duracao = 1.0f;
    private Image img;
    private void Awake()
    {
         
        img = GetComponent<Image>();
    }

    public void Aparecer()
    {
        transform.DOScale(1.2f,duracao);
        Sequence anim = DOTween.Sequence();
        anim.Append(img.DOColor(new Color(255,255,255,1.0f),duracao))
            .Append(transform.DOScale(1.0f,duracao));
        
    } 
}
