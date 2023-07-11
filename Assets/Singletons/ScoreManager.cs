using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
	public static ScoreManager instance;
	public TextMeshProUGUI score, bestScore;

	private void Awake()
	{
		instance = this;
	}
	public void BestScoreSave(string level, int pt )
	{
		if(!ZPlayerPrefs.HasKey(level + "best" + OndeEstou.instance.faseMestra) )
		{
			ZPlayerPrefs.SetInt(level + "best" + OndeEstou.instance.faseMestra, pt);
		} else
		{
			if(GameManager.instance.Score > ZPlayerPrefs.GetInt(level + "best" + OndeEstou.instance.faseMestra) )
			{
				ZPlayerPrefs.SetInt(level + "best" + OndeEstou.instance.faseMestra, GameManager.instance.Score);
			}
		}
	}
	public int BestScoreLoad(string level )
	{
		if(ZPlayerPrefs.HasKey(level + "best" + OndeEstou.instance.faseMestra ) )
		{
			return ZPlayerPrefs.GetInt(level + "best" + OndeEstou.instance.faseMestra );
		}
		else
		{
			return 0;
		}
		
	}
	public void AtualizarScore()
	{
		int novoScore = GameManager.instance.Score;
		score.text = novoScore.ToString();
		bestScore.text = GameManager.instance.BestScore.ToString();
		if (novoScore > int.Parse(bestScore.text))
		{
			bestScore.text = score.text;
		}
	}
}
