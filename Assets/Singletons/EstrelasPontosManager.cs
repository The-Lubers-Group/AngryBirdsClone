using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EstrelasPontosManager : MonoBehaviour
{
	private TextMeshProUGUI estrelas1, estrelas2, pontos1, pontos2;
	private int[] estrelasVal;
	private int[] pontosVal;
	private void Awake()
	{
		estrelasVal = new int[2];
		pontosVal = new int[2];

		for (int i = 0; i < 2; i++)
		{
			if(ZPlayerPrefs.HasKey("FasesNumMestra" + ( i + 1 ) ))
			{
				for(int j = 0; j <= ZPlayerPrefs.GetInt("FasesNumMestra" + (i+1)); j++ )
				{
					if(ZPlayerPrefs.HasKey( "Level" + j + "_Mestra" + ( i + 1 ) + "estrelas" ) )
					{
						estrelasVal[i] += ZPlayerPrefs.GetInt("Level" + j + "_Mestra" + ( i + 1 ) + "estrelas");
						ZPlayerPrefs.SetInt("Mestra" + (i + 1) + "Star" , estrelasVal[i]);
					}

					if ( ZPlayerPrefs.HasKey( "Level" + j + "_Mestra" + ( i + 1 ) + "bestMestra" + ( i + 1 ) )  )
					{

						pontosVal[ i ] += ZPlayerPrefs.GetInt( "Level" + j + "_Mestra" + ( i + 1 ) + "bestMestra" + ( i + 1 ) );
						ZPlayerPrefs.SetInt( "Mestra" + ( i + 1 ) + "p", pontosVal[ i ] ); 
					}

				}
			}
			
		}
		estrelas1 = GameObject.FindWithTag("UITextStarsMestre1").GetComponent<TextMeshProUGUI>();
		estrelas2 = GameObject.FindWithTag("UITextStarsMestre2").GetComponent<TextMeshProUGUI>();
		if (ZPlayerPrefs.HasKey("Mestra1Star")) 
		{
			estrelas1.SetText(ZPlayerPrefs.GetInt("Mestra1Star").ToString());
		}
		if (ZPlayerPrefs.HasKey("Mestra2Star")) 
		{
			estrelas2.SetText(ZPlayerPrefs.GetInt("Mestra2Star").ToString());
		}
		pontos1 = GameObject.FindWithTag("UITextScoreMestre1").GetComponent<TextMeshProUGUI>();
		pontos2 = GameObject.FindWithTag("UITextScoreMestre2").GetComponent<TextMeshProUGUI>();

		if (ZPlayerPrefs.HasKey("Mestra1p")) 
		{
			pontos1.SetText(ZPlayerPrefs.GetInt("Mestra1p").ToString());
		}
		if (ZPlayerPrefs.HasKey("Mestra2p")) 
		{
			pontos2.SetText(ZPlayerPrefs.GetInt("Mestra2p").ToString());
		}
	}

}
