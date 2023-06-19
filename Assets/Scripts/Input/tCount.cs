using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class tCount : MonoBehaviour
{
    public TextMeshProUGUI txt;
    public int toques;
    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)
        {
            toques += Input.touchCount;
            txt.text = Input.touchCount.ToString();
        }
    }
}
