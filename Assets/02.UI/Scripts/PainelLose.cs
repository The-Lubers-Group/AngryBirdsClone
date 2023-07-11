using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PainelLose : Painel
{
	public static PainelLose instance;
    // Start is called before the first frame update
    public override void Awake()
    {
		base.Awake();
		instance = this;
		
    }
	
}
