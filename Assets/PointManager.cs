using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour
{
	public static PointManager instance;
	private void Awake()
	{
		if(instance == null )
		{
			instance = this;
			DontDestroyOnLoad( this.gameObject );
		}
		else
		{
			Destroy(this.gameObject);
		}
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
}
