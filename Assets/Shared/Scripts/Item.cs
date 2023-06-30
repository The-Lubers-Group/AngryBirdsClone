using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
	public int Custo = 50;
	public void Comprar()
	{
		if(CoinManager.instance.LoadDados() >= Custo)
		{
			CoinManager.instance.RetirarMoedas(Custo);

		}
		else
		{
			print("Sem Money");
		}
	}
}
