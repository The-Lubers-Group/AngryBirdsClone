using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStore : MonoBehaviour
{
	public int Custo = 50;
	public UsableObjectsEnum objectType;
	public void Comprar()
	{
		if(CoinManager.instance.LoadDados() >= Custo)
		{
			CoinManager.instance.RetirarMoedas(Custo);
			string key = "";
			int qtdAtual = 0;
			switch(objectType)
			{

				case UsableObjectsEnum.Mira:
					key = "miraObjects";
					break;
			}
			if(ZPlayerPrefs.HasKey(key))
			{
				qtdAtual = ZPlayerPrefs.GetInt(key);
				qtdAtual++;
				ZPlayerPrefs.SetInt(key, qtdAtual);
			} 
			else
			{
				qtdAtual++;
				ZPlayerPrefs.SetInt(key, qtdAtual);
			}
		}
		else
		{
			CoinManager.instance.NotMoney();
		}
	}
}
