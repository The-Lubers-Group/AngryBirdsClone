using UnityEngine;


public class MoveTxtInfo : MonoBehaviour
{
    private Vector3 pos;
    private RectTransform rt;   
    private bool libera= false;
    public RectTransform canvasBack;

    private void Awake()
    {

        rt = GetComponent<RectTransform>();
        pos = rt.anchoredPosition;

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

        if(!RectOverlap(canvasBack, rt))
        {
            rt.anchoredPosition = pos;
        }
    }
    public void LiberaMove()
    {
        libera = true;
       
    }
    public void BlockMove()
    {
        libera = false;
    }
    bool RectOverlap(RectTransform rectTrans1, RectTransform rectTrans2)
    {
        Rect rect1 = new Rect(rectTrans1.localPosition.x, rectTrans1.localPosition.y,rectTrans1.rect.width,rectTrans1.rect.height);
        Rect rect2 = new Rect(rectTrans2.localPosition.x, rectTrans2.localPosition.y,rectTrans2.rect.width,rectTrans2.rect.height);
        return rect1.Overlaps(rect2);
    }
}
