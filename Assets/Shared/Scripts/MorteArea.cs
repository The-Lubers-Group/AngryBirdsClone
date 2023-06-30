using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MorteArea : MonoBehaviour
{
	private void OnCollisionEnter2D( Collision2D collision )
	{
		if (collision.gameObject.CompareTag("Passaro"))
			{
            Destroy(collision.gameObject);
			}
		else if (collision.gameObject.CompareTag("Destrutivel"))
			{
			ImpactCod obj = collision.gameObject.GetComponent<ImpactCod>();
			obj.ProcesarMorte();
			}
		else if (collision.gameObject.CompareTag("InimigoPorco"))
			{
			ImpactAnimaPorco obj = collision.gameObject.GetComponent<ImpactAnimaPorco>();
			obj.ProcesarMorte();
			}
		else if (collision.gameObject.CompareTag("Player"))
			{
			Drag drag = collision.gameObject.GetComponent<Drag>();
			drag.MataPassaro();
			}
	}
}
