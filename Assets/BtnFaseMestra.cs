using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnFaseMestra : MonoBehaviour
{
    private Button btnFase;

	private void Awake()
	{
		btnFase = GetComponent<Button>();
		btnFase.onClick.AddListener(GoToLevels);
	}
	private void GoToLevels()
	{
		OndeEstou.instance.Mestra(gameObject.name);
	}
}
