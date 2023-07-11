using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainelPause : Painel
{
	public override void Awake()
	{
		base.Awake();
	}
	public override void OnFinishOpen( int dados )
	{
		GameManager.instance.pausado = true;
		Time.timeScale = 0;
		fundoPreto.enabled = true;
		camDrag.enabled = false;
		Catapult.instance.LineRendererDisabled();
	}
	public void OnPause()
	{
		openMenu(0,0);
	}
	public void OnContinue()
	{
		GameManager.instance.pausado = false;
		Unpaused();
		fundoPreto.enabled = false;
		camDrag.enabled = true;
		Catapult.instance.LineRendererEnabled();
		anim.Play(nameAnimation+"Inverse");
	}
	public void Unpaused()
	{
		Time.timeScale = 1;
	}
}
