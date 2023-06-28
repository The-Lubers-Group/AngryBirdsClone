using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Money : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI textMoeda;
	private int valor;
	private void Awake()
	{
		textMoeda = GetComponent<TextMeshProUGUI>();
	}
	private void Start()
	{
		valor = CoinManager.instance.LoadDados();
		textMoeda.text = valor.ToString();
	}
	public void AtualizaValor()
	{
		textMoeda.text = CoinManager.instance.LoadDados().ToString();
	}
}
