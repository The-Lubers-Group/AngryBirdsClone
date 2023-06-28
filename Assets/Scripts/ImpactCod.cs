using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactCod : MonoBehaviour
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

    // Start is called before the first frame update
    void Start()
    {
        limite = 0;
        spriteR = GetComponent<SpriteRenderer>();
        spriteR.sprite = sprites[0];
		audioObj = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.magnitude > 4 && collision.relativeVelocity.magnitude < 10)
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
				ProcesarMorte();
            }
        }
        else if(collision.relativeVelocity.magnitude > 12 && collision.gameObject.CompareTag("Player"))
        {
         ProcesarMorte();  
        }
    }
	void ProcesarMorte()
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
			audioObj.clip = clips[1];
			audioObj.Play();
			GameManager.instance.Score = Score;
			Destroy(gameObject,.5f);
		}
		
	}
}
