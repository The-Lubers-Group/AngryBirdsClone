using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstrelasAnimController : MonoBehaviour
{
	public static EstrelasAnimController instance;
    public EstrelaAnim estrela1,estrela2,estrela3;
	public AudioClip[] audios = new AudioClip[3];
    public int Qtd;
	public bool Ready;
	private void Awake()
	{
	 
		if(instance == null )
		{
			instance = this;
		}
		else
		{
			Destroy( this.gameObject );
		}
	}
	// Start is called before the first frame update
	public void startStars(int qtd)
    {
		Qtd = qtd;
        StartCoroutine(ComecarAnimacoes());
    }
    IEnumerator ComecarAnimacoes()
    {
		yield return new WaitUntil(() => Ready);
		yield return new WaitForSeconds(1f);
		
        if (Qtd > 0)
        {
            estrela1.Aparecer();
			AudioManager.instance.PlayEffect(audios[0]);
			yield return new WaitForSeconds(.8f);
        }
        if (Qtd > 1)
        {
            estrela2.Aparecer();
			AudioManager.instance.PlayEffect(audios[1]);
			yield return new WaitForSeconds(.8f);
        }
        if(Qtd > 2)
		{
            estrela3.Aparecer();
			AudioManager.instance.PlayEffect(audios[2]);	
        }
		PainelWin.instance.ActivateButtons();
        yield return null;

    }
	void setReady()
	{
		Ready = true;
	}
}
