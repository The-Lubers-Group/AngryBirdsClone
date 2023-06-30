using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstrelasAnimController : MonoBehaviour
{
	public static EstrelasAnimController instance;
    public EstrelaAnim estrela1,estrela2,estrela3;
	public AudioSource estrelaAudio1,estrelaAudio2,estrelaAudio3;
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

		estrela1 = GameObject.Find("FullStar1").GetComponent<EstrelaAnim>();
		estrela2 = GameObject.Find("FullStar2").GetComponent<EstrelaAnim>();
		estrela3 = GameObject.Find("FullStar3").GetComponent<EstrelaAnim>();

		estrelaAudio1 = GameObject.Find("FullStar1").GetComponent<AudioSource>();
		estrelaAudio2 = GameObject.Find("FullStar2").GetComponent<AudioSource>();
		estrelaAudio3 = GameObject.Find("FullStar3").GetComponent<AudioSource>();
	}
	// Start is called before the first frame update
	public void IniciarCoroutine()
    {
        StartCoroutine(ComecarAnimacoes());
    }
    IEnumerator ComecarAnimacoes()
    {
		yield return new WaitUntil(() => Ready);
		yield return new WaitForSeconds(1f);
		
        if (Qtd > 0)
        {
            estrela1.Aparecer();
			estrelaAudio1.Play();
			yield return new WaitForSeconds(.8f);
        }
        if (Qtd > 1)
        {
            estrela2.Aparecer();
			estrelaAudio2.Play();

			yield return new WaitForSeconds(.8f);
        }
        if(Qtd > 2)
		{
            estrela3.Aparecer();
			estrelaAudio3.Play();

        }
		UIManager.instance.winBtnMenu.interactable = true;
		UIManager.instance.winBtnNovamente.interactable = true;
		UIManager.instance.winBtnProximo.interactable = true;
        yield return null;

    }
	void setReady()
	{
		Ready = true;
	}
}
