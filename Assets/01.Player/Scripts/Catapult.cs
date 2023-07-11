using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catapult : MonoBehaviour
{
	public static Catapult instance;
	private LineRenderer frontLR;
	private LineRenderer backLR;
	public PolygonCollider2D colisorCompleto;
	private Rigidbody2D catapultRB;
	private Ray leftCatapultRay;
	public float maxDistanceDragged = 3.0f;
	public float minDistanceDragged = 1.0f;
	[SerializeField] public float constanteResorte = 8.0f;

	private void Awake()
	{
		instance = this;

		foreach (LineRenderer lr in GetComponentsInChildren<LineRenderer>())
		{
			if (lr.transform.position.z > 0 )
			{
				backLR = lr;
			} else
			{
				frontLR = lr;
			}
		}
		colisorCompleto = backLR.GetComponent<PolygonCollider2D>();
		catapultRB = backLR.GetComponent<Rigidbody2D>();
		leftCatapultRay = new Ray( GetPosition(), Vector3.zero );
		
	}
	public void LineUpdate()
	{
			//Obtendo a Direção de Estilingue até o Passaro
			Vector3 catapultToBird = GameManager.instance.passaroAtual.transform.position - GetPosition();
			//Asignando a direção que esta apontando para o passaro
			leftCatapultRay.direction = catapultToBird;
			//Obtendo do Raio um ponto desse raio, somando a distancia que leva desde a estilingue até o passaro e adicionamos o raio do seu colisor para cobrir o passaro completamente.  
			Vector3 pointL = leftCatapultRay.GetPoint( catapultToBird.magnitude + (GameManager.instance.passaroAtual.passaroCol.radius * GameManager.instance.passaroAtual.transform.localScale.y));
			pointL.z = 0;
			//Atualiza posição da linha com respeito a posição do passaro.
			Catapult.instance.SetupLine(pointL);
	}

	public void LineRendererDisabled()
	{	
		frontLR.enabled = false;
		backLR.enabled = false;
	}
	public void LineRendererEnabled()
	{
		if(!GameManager.instance.passaroAtual.passaroLancado)
		{
			frontLR.enabled = true;
			backLR.enabled = true;
		}
	}
	public void SetupLine(Vector3 point)
	{
		frontLR.SetPosition( 1, point );
		backLR.SetPosition( 1, point );
	}
	public Vector3 GetPosition()
	{
		return catapultRB.transform.position;
	}
}
