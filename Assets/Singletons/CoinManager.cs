using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using TMPro;

public class CoinManager : MonoBehaviour
{
	public static CoinManager instance;
	public Money moneyDisplay = null;
	public TextMeshProUGUI textLoja;
	public Animator animatorTextLoja;
	private void Awake()
	{
		if (instance == null )
		{
			instance = this;
			DontDestroyOnLoad( this.gameObject );
		}
		else
		{
			Destroy(this.gameObject);
		}
		
	}
	private void OnEnable()
	{
		SceneManager.sceneLoaded += Carrega;
	}
	private void OnDisable()
	{
		SceneManager.sceneLoaded -= Carrega;
	}
	private void Carrega(Scene cena, LoadSceneMode modo)
	{
		if(cena.name == "Loja" || cena.name == "MenuFases")
		{
			moneyDisplay = GameObject.FindWithTag("UIMoeda").GetComponent<Money>();
			if (cena.name == "Loja")
			{
				textLoja = GameObject.FindWithTag("UILojaText").GetComponent<TextMeshProUGUI>();
				animatorTextLoja = textLoja.GetComponent<Animator>();
			}
		}
		
	}
	public void SalvarDados(int moeda )
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream fs = File.Create(Application.persistentDataPath + "/dadoscoinData.data");

		Dados coin = new Dados();
		coin.moedas = moeda;

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
		textLoja.text = "Comprado";
		animatorTextLoja.Play("notificationAnim");
	}
	public void NotMoney()
	{
		textLoja.text = "Moedas Insuficientes";
		animatorTextLoja.Play("notificationAnim");
	}

	public void setText(string txt)
	{
		textLoja.text = txt;
	}

	[Serializable]
	class Dados
	{

		public int moedas;
	}
}
