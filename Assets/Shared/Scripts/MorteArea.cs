using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MorteArea : MonoBehaviour
{
	private void OnCollisionEnter2D( Collision2D collision )
	{
		if(collision.gameObject.CompareTag("Passaro"))
		{
			Destroy( collision.gameObject );
		}
		else if (collision.gameObject.CompareTag("Destrutivel") || collision.gameObject.CompareTag("InimigoPorco"))
		{
			IDamageable obj = collision.gameObject.GetComponent<IDamageable>();
			obj.ProcessarMorte();
		}
		else if (collision.gameObject.CompareTag("Player"))
		{
			Drag drag = collision.gameObject.GetComponent<Drag>();
			drag.StartCoroutine(drag.TempoMorte());
		}
	}
}
