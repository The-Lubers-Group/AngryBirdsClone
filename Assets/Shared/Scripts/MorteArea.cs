using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MorteArea : MonoBehaviour
{
	private void OnCollisionEnter2D( Collision2D collision )
	{
		if(collision.gameObject.CompareTag(Constants.PASSAROUI_TAG))
		{
			Destroy( collision.gameObject );
		}
		else if (collision.gameObject.CompareTag("Destrutivel") || collision.gameObject.CompareTag(Constants.INIMIGOPORCO_TAG))
		{
			IDamageable obj = collision.gameObject.GetComponent<IDamageable>();
			obj.ProcessarMorte();
		}
		else if (collision.gameObject.CompareTag(Constants.PLAYER_TAG))
		{
			Passaro passaro = collision.gameObject.GetComponent<Passaro>();
			passaro.estaMorrendo = true;
			passaro.continueToRecharge();
			passaro.StartCoroutine(passaro.TempoMorte());
		}
	}
}
