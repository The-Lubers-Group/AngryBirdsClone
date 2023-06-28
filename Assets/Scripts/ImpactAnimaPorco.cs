using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ImpactAnimaPorco : MonoBehaviour
{
	Animator animacoes;
	int limite = -1;
	public int Score = 5000;
	public string[] clips;
	private bool vivo = true;
	[SerializeField] GameObject EfeitoDestruido, EfeitoScore1000, EfeitoScore5000;
    // Start is called before the first frame update
    void Start()
    {
        animacoes = GetComponent<Animator>();
    }

	 private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.magnitude > 4 && collision.relativeVelocity.magnitude < 12)
		{
            if (limite < clips.Length - 1)
			{
                limite++;
				animacoes.Play(clips[limite]);
            }
            else if(limite ==  clips.Length - 1)
            {
                ProcesarMorte();
            }
        }
        else if(collision.relativeVelocity.magnitude > 12 || collision.gameObject.CompareTag("Player"))
        {
            ProcesarMorte(); 
		}
    }

	void ProcesarMorte()
	{
		if ( vivo )
		{
			vivo = false;
			Instantiate(EfeitoDestruido, new Vector2(transform.position.x,transform.position.y), Quaternion.identity);
			if (Score == 1000)
			{
				Instantiate(EfeitoScore1000, new Vector2(transform.position.x,transform.position.y), Quaternion.identity);
			}
			else
			{
				Instantiate(EfeitoScore5000, new Vector2(transform.position.x,transform.position.y), Quaternion.identity);
                    
			}
		
		
			GameManager.instance.numPorcosCena--;
			GameManager.instance.Score = Score;
			Destroy(gameObject);
		}
    
	}
	
}
