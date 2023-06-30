using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomB : MonoBehaviour
{
    public float orthoZoomSpeed = 0.05f;
    public Camera cam;
    private Touch toque0, toque1;
    private Vector2 touchZeroPrevPos, touchOnePrevPos;
    private float prevTouchDeltaMag, touchDeltaMag, deltaMagnitudeDiff;


    
    // Update is called once per frame
    void Update()
    {
		if(!GameManager.instance.pausado)
		{
			if (Input.touchCount == 2)
			{
				toque0 = Input.GetTouch(0);
				toque1 = Input.GetTouch(1);

				touchZeroPrevPos = toque0.position - toque0.deltaPosition;
				touchOnePrevPos = toque1.position - toque1.deltaPosition;

				prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
				touchDeltaMag = (toque0.position - toque1.position).magnitude;

				deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
				if (cam.orthographic)
				{
					cam.orthographicSize += deltaMagnitudeDiff * (orthoZoomSpeed*Time.deltaTime);
					cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, 5f, 10f);
               
				}
			}
		}
    }
}
