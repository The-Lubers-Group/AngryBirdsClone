using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineR : MonoBehaviour
{
    public Transform p2, p3;
    public LineRenderer lR;

    // Start is called before the first frame update
    void Start()
    {
        lR = GetComponent<LineRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        lR.SetPosition(0,transform.position);
        lR.SetPosition(1,p2.transform.position);
        lR.SetPosition(2,p3.transform.position);
    }
}
