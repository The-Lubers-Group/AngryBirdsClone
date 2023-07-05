using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ImpactAnimaPorco : MonoBehaviour, IDamageable
{
	Animator animacoes;
	int limite = -1;
	public int Score = 5000;
	public string[] clips;
	private bool vivo = true;
	public float resistenciaMax = 70f;
	public float resistenciaMin = 16f;
		
	[SerializeField] GameObject EfeitoDestruido, EfeitoScore1000, EfeitoScore5000;
   
    void Awake()
    {
        animacoes = GetComponent<Animator>();
    }

	public void Danificar()
	{
            if (limite < clips.Length - 1)
			{
                limite++;
				animacoes.Play(clips[limite]);
            }
            else if(limite ==  clips.Length - 1)
            {
                ProcessarMorte();
            }
	}
	 private void OnCollisionEnter2D(Collision2D collision)
    {
        if ( collision.rigidbody != null )
		{
			Damage(collision.rigidbody);
		}
		else
		{
			if(collision.gameObject.name == "Floor" )
			{
				if(collision.relativeVelocity.sqrMagnitude > resistenciaMin && collision.relativeVelocity.sqrMagnitude <= resistenciaMax )
				{
					Danificar();
				}
				else if(collision.relativeVelocity.sqrMagnitude > resistenciaMax)
				{
					ProcessarMorte();
				}
			}
		}
    }
	public void Damage(Rigidbody2D rigidB)
	{
		if ((rigidB.velocity.sqrMagnitude > resistenciaMin && rigidB.velocity.sqrMagnitude <= resistenciaMax) || rigidB.gameObject.CompareTag("Bomba"))
        {
            Danificar();
        }
        else if(rigidB.velocity.sqrMagnitude > resistenciaMax || rigidB.gameObject.CompareTag("Player") || rigidB.gameObject.CompareTag("clone"))
        {
			ProcessarMorte();  
        }
	}
	public void ProcessarMorte()
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
