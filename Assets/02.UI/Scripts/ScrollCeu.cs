using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScrollCeu : MonoBehaviour
{
    public RawImage back;
    // Update is called once per frame
    private void Awake()
    {
        back = GetComponent<RawImage>();
    }
    void Update()
    {
        back.uvRect = new Rect(0.01f * Time.time,0,1,1);
    }
}
