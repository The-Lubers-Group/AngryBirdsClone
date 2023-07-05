using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class BombaPassaro : MonoBehaviour
{
	public Rigidbody2D passaroRB;
	public SpriteRenderer passaroSR; 
	public CircleCollider2D passaroCol; 
	public bool libera = false;
	public int trava = 0;
	private Touch touch;
	public GameObject bomba;
	public float forcaExplosao = 10f;
	public float raioExplosao = 5f;

	// Start is called before the first frame update
	void Awake()
	{
		passaroRB = GetComponent<Rigidbody2D>();
		passaroSR = GetComponent<SpriteRenderer>();
		passaroCol = GetComponent<CircleCollider2D>();
	}

	// Update is called once per frame
	void Update()
	{
		if ( passaroRB != null )
		{
#if UNITY_EDITOR

			if ( Input.GetMouseButtonDown( 0 ) && !passaroRB.isKinematic && trava == 0 )
			{
				libera = true;
				trava = 1;
				Explode();
			}

#endif
#if UNITY_ANDROID
			if ( Input.touchCount > 0 )
			{

				touch = Input.GetTouch( 0 );
				if ( touch.phase == TouchPhase.Ended && trava < 2 && !passaroRB.isKinematic )
				{
					trava++;
					if ( trava == 2 )
					{

						libera = true;
						Explode();
					}
				}
			}

#endif
		}

	}
	private void FixedUpdate()
	{

		if ( libera )
		{
			if ( passaroRB != null )
			{
			//	passaroRB.velocity = passaroRB.velocity * 2.5f;
				libera = false;
			}
		}
	}
	void Explode()
	{
		GameObject bmb = Instantiate( bomba, transform.position, Quaternion.identity );
		Collider2D[] inExplosionRadius = Physics2D.OverlapCircleAll( transform.position, raioExplosao );
		passaroCol.enabled = false;
		passaroRB.bodyType = RigidbodyType2D.Static;
		string tagTemp = passaroRB.gameObject.tag;
		passaroRB.gameObject.tag = bmb.gameObject.tag;
		passaroSR.enabled = false;
		foreach ( Collider2D obj in inExplosionRadius )
		{
			if (obj != null)
			{
				Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
				IDamageable damageable = obj.GetComponent<IDamageable>();
				damageable?.Damage(passaroRB);
				if ( rb != null )
				{
					Vector2 distanceVector = obj.transform.position - transform.position;
					if ( distanceVector.magnitude > 0 )
					{
						float explosionForce = forcaExplosao / distanceVector.magnitude;
						rb.AddForce( distanceVector.normalized * explosionForce, ForceMode2D.Impulse );
					}
				}
			}
		}
		passaroRB.gameObject.tag = tagTemp;
	
	}
	//private void OnDrawGizmos()
	//{
	//	Gizmos.DrawWireSphere( transform.position, raioExplosao );
	//}
}
