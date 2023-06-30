using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class btnAudioToggleIcon : MonoBehaviour
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
		if ( UIManager.instance.somActivo )
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
			UIManager.instance.somActivo = false;
		}
		else
		{
			img.sprite = iconSound;
			UIManager.instance.somActivo = true;
		}
	}

}
