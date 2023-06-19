using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSegue : MonoBehaviour
{
    [SerializeField]
    private Transform objE, objD, passaro;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(passaro != null)
        {
            Vector3 posCam = transform.position;
            posCam.x = passaro.position.x;
            posCam.x = Mathf.Clamp(posCam.x, objE.position.x, objD.position.x);
            transform.position = posCam;
        }
       
    }
}
