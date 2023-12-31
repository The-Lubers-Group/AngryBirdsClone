using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Veloz : MonoBehaviour
{

    public Rigidbody2D passaroRB;
    public bool libera = false;
    public int trava = 0;
    private Touch touch;
	[SerializeField] private float fatorVelocidade = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        passaroRB = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        #if UNITY_EDITOR
        
        if(Input.GetMouseButtonDown(0) && !passaroRB.isKinematic && trava == 0)
        {
            libera = true;
            trava = 1;
        }
        
        #endif
        #if UNITY_ANDROID
        if(Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Ended && trava < 2 && !passaroRB.isKinematic)
            {
                trava++;
                if(trava == 2)
                {
                    libera = true;
                }
            }
        }

        #endif
    }
    private void FixedUpdate()
    {
        if (libera)
        {
            passaroRB.velocity = passaroRB.velocity * fatorVelocidade;
            libera = false;
        }
    }
}
