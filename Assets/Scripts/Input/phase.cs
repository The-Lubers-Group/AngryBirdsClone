using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class phase : MonoBehaviour
{
    public TextMeshProUGUI txt;
    public Touch toque;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            toque = Input.GetTouch(0);
            switch(toque.phase)
            {
                case TouchPhase.Began:
                    txt.text = "Began";
                    break;
                case TouchPhase.Ended:
                    txt.text = "Ended";
                    break;
                case TouchPhase.Stationary:
                    txt.text = "Stationary";
                    break;
                case TouchPhase.Moved:
                    txt.text = "Moved";
                    break;
                case TouchPhase.Canceled:
                    txt.text = "Canceled";
                    break;
            }
        }
    }
}
