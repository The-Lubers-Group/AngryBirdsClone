using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Mira : MonoBehaviour
{
	public static Mira instance;
	public bool mirando = false;
	public GameObject dotsContainer;
	[SerializeField] private GameObject dotPrefab;
	private List<GameObject> caminho;
	private List<Renderer> caminhoRender = new List<Renderer>();
	
	public int pointsNum = 30;
	private void Awake()
	{
		instance = this;
	}
	private void Start()
	{
		StartMira();
	}
	public void Mirando(Vector2 forca, float masa, Vector3 pos)
	{
		if(Input.GetMouseButton(0))
		{
			//print("Apreto el mouse");
			if (mirando == false) {
				ligarCaminho();
				CalculoCaminho (forca, masa, pos);
			} else {
				//print("mirando es verdadero");
				CalculoCaminho (forca, masa, pos);
			}
		}
	}

	void StartMira()
    {
		if (dotsContainer.transform.Cast<Transform>().Count() == 0)
		{
			for ( int i = 0; i < pointsNum; i++ )
			{
				GameObject local = Instantiate(dotPrefab, Vector3.zero, Quaternion.identity, dotsContainer.transform);
				Renderer render = local.GetComponent<Renderer>();
				caminhoRender.Add(render);
				render.enabled = false;
			}
			caminho = dotsContainer.transform.Cast<Transform>().ToList().ConvertAll(t => t.gameObject);
		} else
		{
			caminho = dotsContainer.transform.Cast<Transform>().ToList().ConvertAll(t => t.gameObject);
			for ( int i = 0; i < pointsNum; i++ )
			{
				Renderer render = caminho[i].GetComponent<Renderer>();
				caminhoRender.Add(render );
				render.enabled = false;
			}
		}
		
    }
		// Metodos do objeto Mira
	public void ligarCaminho()
	{
		mirando = true;
		foreach ( Renderer item in caminhoRender )
		{
			item.enabled = true;
		}
	}
	public void desligarCaminho()
	{
		mirando =false;
		foreach ( Renderer item in caminhoRender )
		{
			item.enabled = false;
		}
	}
	Vector2 CaminhoPonto(Vector2 posInicial,Vector2 velInicial,float tempo)
	{
		return posInicial + velInicial * tempo + 0.5f * Physics2D.gravity * tempo * tempo;
	}
	void CalculoCaminho(Vector2 forca, float masa, Vector3 pos)
	{
			Vector2 vel = forca / masa;
			for(int x=0; x < pointsNum; x++)
			{
				float t = (x * Time.fixedDeltaTime * 12);
				Vector3 point = CaminhoPonto (pos, vel, t);
				point.z = 0f;
				caminho [x].transform.position = point;
				
			}
		
	}
}
