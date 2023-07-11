using System;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;

public class CoinManager : MonoBehaviour
{
	public static CoinManager instance;
	public Money moneyDisplay = null;
	public TextMeshProUGUI notificationText;
	public Animator animatorTextLoja;
	private void Awake()
	{
		instance = this;
		moneyDisplay = GetComponentInChildren<Money>();
	}
	public void Start()
	{
		if (OndeEstou.instance.faseN == "Loja")
		{
			notificationText = GameObject.FindWithTag("UILojaText").GetComponent<TextMeshProUGUI>();
			animatorTextLoja = notificationText.GetComponent<Animator>();
		}
	}
	public void SalvarDados(int moedas)
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream fs = File.Create(Application.persistentDataPath + "/dadoscoinData.data");

		Dados coin = new Dados();
		coin.moedas = moedas;

		bf.Serialize(fs, coin);
		fs.Close();
	}

	public int LoadDados()
	{
		int moeda = 0;
		if(File.Exists(Application.persistentDataPath + "/dadoscoinData.data" ) )
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream fs = File.Open(Application.persistentDataPath + "/dadoscoinData.data" , FileMode.Open);
			Dados coin = (Dados) bf.Deserialize(fs);
			fs.Close();
			moeda = (int) coin.moedas;
		}
		return moeda;
	}
	public void RetirarMoedas(int valor)
	{
		int tempMoedasTotal, novoVal;
		tempMoedasTotal = LoadDados();
		novoVal = tempMoedasTotal - valor;
		SalvarDados(novoVal);
		moneyDisplay?.AtualizaValor();
		if( notificationText != null)
		{
			notificationText.text = "Comprado";
			animatorTextLoja.Play("notificationAnim");
		}	
	}
	public void NotMoney()
	{
		if( notificationText != null)
		{
			notificationText.text = "Moedas Insuficientes";
			animatorTextLoja.Play("notificationAnim");
		}
	}

	public void setText(string txt)
	{
		if( notificationText != null)
		{
			notificationText.text = txt;
		}
	}
	public int GetMoneyTemp()
	{
		return moneyDisplay.GetMoneyTemp();
	}

	[Serializable]
	class Dados
	{
		public int moedas;
	}
}
