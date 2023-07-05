using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum UsableObjectsEnum
{
	Mira
}
public class UsableObjectsManager : MonoBehaviour
{
	public static UsableObjectsManager instance;
	[SerializeField]private UsableObject[] objects;
    private void Awake()
	{
		if ( instance == null )
		{
			instance = this;
		}
		else
		{
			Destroy( this.gameObject );
		}
		objects = GameObject.FindObjectsOfType<UsableObject>();
	}
	//Indica ao game manager para ativar o objeto
	public void SetActiveObject( UsableObjectsEnum obj )
	{
		switch ( obj )
		{
			case UsableObjectsEnum.Mira:
				GameManager.instance.mira = true;
				break;
		}
	}

	//Indica ao game manager para desactivar o objeto
	public void SetInactiveObject( UsableObjectsEnum obj )
	{
		switch ( obj )
		{
			case UsableObjectsEnum.Mira:
				GameManager.instance.mira = false;
				break;
		}
	}
	//Ativação do objeto comprobando se foi possivel o seu uso, se não ele desativa o objeto
	public void ObjectUsed( UsableObjectsEnum type )
	{
		foreach ( UsableObject obj in objects )
		{
			if (obj.objectType == type)
			{
				if(obj.UseObject())
				{
					SetActiveObject(type);
				}
				else
				{
					SetInactiveObject(type);
				}
				break;
			}
		}
	}

	
}
