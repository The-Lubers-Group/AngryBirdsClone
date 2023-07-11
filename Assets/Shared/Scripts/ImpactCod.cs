using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactCod : MonoBehaviour, IDamageable
{
    private int limite;
    public int Score = 1000;
    private SpriteRenderer spriteR;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private GameObject EfeitoDestruido;
    [SerializeField] private GameObject EfeitoScore1000;
    [SerializeField] private GameObject EfeitoScore5000;
	[SerializeField] private AudioSource audioObj;
	[SerializeField] private AudioClip[] clips;
	private bool vivo = true;
	public float resistenciaMax = 70f;
	public float resistenciaMin = 16f;

    void Awake()
    {
        limite = 0;
        spriteR = GetComponent<SpriteRenderer>();
        spriteR.sprite = sprites[0];
		audioObj = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
		if ( collision.rigidbody != null )
		{
			Damage(collision.rigidbody);
		}
		else
		{
			if(collision.gameObject.CompareTag(Constants.FLOOR_TAG) )
			{
				if(collision.relativeVelocity.sqrMagnitude > 16 && collision.relativeVelocity.sqrMagnitude <= resistenciaMax )
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
	public void Danificar()
	{
		if (limite < sprites.Length - 1)
        {
            limite++;
            spriteR.sprite = sprites[limite];
			audioObj.clip = clips[0];
			audioObj.Play();
        }
        else if(limite ==  sprites.Length - 1)
        {
			ProcessarMorte();
        }
	}
	public void Damage(Rigidbody2D rigidB)
	{
        if ((rigidB.velocity.sqrMagnitude > resistenciaMin && rigidB.velocity.sqrMagnitude <=	resistenciaMax ) || rigidB.gameObject.CompareTag(Constants.BOMB_TAG))
        {
            Danificar();
        }
        else if(rigidB.velocity.sqrMagnitude > resistenciaMax && ( rigidB.gameObject.CompareTag("Player") || rigidB.gameObject.CompareTag(Constants.CLONE_TAG)))
        {
			ProcessarMorte();  
        }
	}
	public void ProcessarMorte()
	{
		if (vivo)
		{
			Instantiate(EfeitoDestruido, new Vector2(transform.position.x,transform.position.y), Quaternion.identity);
			if (Score == 1000)
			{
				Instantiate(EfeitoScore1000, new Vector2(transform.position.x,transform.position.y), Quaternion.identity);
			}
			else
			{
				Instantiate(EfeitoScore5000, new Vector2(transform.position.x,transform.position.y), Quaternion.identity);
			}
			AudioManager.instance.PlayEffect(clips[1]);
			GameManager.instance.Score = Score;
			Destroy(gameObject);
		}
		
	}
}
