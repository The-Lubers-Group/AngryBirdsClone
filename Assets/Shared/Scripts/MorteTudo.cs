using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorteTudo : MonoBehaviour
{
	public int morteTime = 1;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(morte());
    }
    IEnumerator morte()
    {
        yield return new WaitForSeconds(morteTime);
        Destroy(gameObject);
    }
	private void OnTriggerEnter2D( Collider2D collision )
	{
		if ( collision.gameObject.CompareTag( "moeda" ) )
		{
			GameManager.instance.moedas += 50;
			UIManager.instance.moedas.text = GameManager.instance.moedas.ToString();
			Destroy(collision.gameObject);
		}
	}
}
