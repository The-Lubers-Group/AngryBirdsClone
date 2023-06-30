using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSegue : MonoBehaviour
{
	private float t = 1;
	public bool inPlace = false;
	// Update is called once per frame
	void Update()
	{
		if ( !GameManager.instance.pausado )
		{
			
			if (  Mathf.FloorToInt(transform.position.x) != Mathf.FloorToInt(GameManager.instance.objE.position.x)  && !GameManager.instance.passaroLancado && !inPlace )
			{
				t -= 0.05f * Time.deltaTime;
				transform.position = new Vector3( Mathf.SmoothStep( GameManager.instance.objE.position.x, Camera.main.transform.position.x, t ), transform.position.y, transform.position.z );
			}
			else
			{
				t = 1;
				inPlace = true;
			}
		}
	}
}
