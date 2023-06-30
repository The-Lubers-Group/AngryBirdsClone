using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveTxtInfo : MonoBehaviour
{
    private Vector3 pos;
    private RectTransform rt;   
    private bool libera= false;
    private GameObject btnBlock, nomeGame;
    private RectTransform uiRect1, uiRect2;

    private void Awake()
    {
        uiRect1 = GameObject.FindWithTag("canvasBack").GetComponent<RectTransform>();
        uiRect2 = GameObject.FindWithTag("infoTxt").GetComponent<RectTransform>();
        rt = GetComponent<RectTransform>();
        pos = rt.anchoredPosition;
        btnBlock =GameObject.FindWithTag("btnBlock");
        btnBlock.SetActive(false);
        nomeGame = GameObject.FindWithTag("nomeGame");
		btnBlock.GetComponent<Button>().onClick.AddListener(BlockMove);
    }

    // Update is called once per frame
    void Update()
    {
        if (libera)
        {
            transform.Translate(0,1*Time.deltaTime,0);
        }
        else
        {
            rt.anchoredPosition = pos;
        }

        if(!RectOverlap(uiRect1, uiRect2))
        {
            rt.anchoredPosition = pos;
        }
    }
    public void LiberaMove()
    {
        btnBlock.SetActive(true);
        nomeGame.SetActive(false);
        libera = true;
        UIManager.instance.PlayAnimBtnConf();
    }
    public void BlockMove()
    {
        btnBlock.SetActive(false);
        nomeGame.SetActive(true);
        libera = false;
    }
    bool RectOverlap(RectTransform rectTrans1, RectTransform rectTrans2)
    {
        Rect rect1 = new Rect(rectTrans1.localPosition.x, rectTrans1.localPosition.y,rectTrans1.rect.width,rectTrans1.rect.height);
        Rect rect2 = new Rect(rectTrans2.localPosition.x, rectTrans2.localPosition.y,rectTrans2.rect.width,rectTrans2.rect.height);
        return rect1.Overlaps(rect2);
    }
}
