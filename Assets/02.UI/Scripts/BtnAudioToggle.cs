using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnAudioToggle : MonoBehaviour
{
	public Sprite iconNotSound;
	public Sprite iconSound;
	public Image img;
	private void Awake()
	{
		img = GetComponent<Image>();
		determinarIcon();
	}
	private void determinarIcon()
	{
		if ( AudioManager.instance.somActivo )
		{
			img.sprite = iconSound;
		}
		else
		{
			img.sprite= iconNotSound;
		}
	}
	public void BtnToggleAudio()
	{
		AudioListener.pause = !AudioListener.pause ;
		if (AudioListener.pause)
		{
			img.sprite = iconNotSound;
			AudioManager.instance.somActivo = false;
		}
		else
		{
			img.sprite = iconSound;
			AudioManager.instance.somActivo = true;
		}
	}

}
