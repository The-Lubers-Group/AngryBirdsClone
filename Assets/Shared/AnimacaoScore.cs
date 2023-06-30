using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnimacaoScore : MonoBehaviour
{
    public float duracao = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer img =GetComponent<SpriteRenderer>();
        Sequence anim = DOTween.Sequence();
        
        anim.Append(transform.DOScale(1.0f, duracao))
            .Insert(0,img.DOColor(Color.white, duracao))
            .AppendInterval(duracao/1.5f)
            .Append(transform.DOScale(0f, duracao))
            .Insert(2,img.DOColor(Color.clear, duracao)).OnComplete(KillObject);
    }
    void KillObject()
    {
       Destroy(gameObject);
    }

}
