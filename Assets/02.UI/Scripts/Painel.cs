using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class  Painel : MonoBehaviour
{
	public Animator anim;
	public string nameAnimation;
	public Image fundoPreto;
	public DragCamera2D camDrag;

	public virtual void Awake()
	{
		anim = GetComponent<Animator>();
		camDrag = Camera.main.GetComponent<DragCamera2D>();
	}
	public void openMenu(float delay,int dados = 0)
	{
		StartCoroutine(DelayAnim(anim, nameAnimation, delay, AbrirMenu));
		OnFinishOpen(dados);
	}
	public virtual void OnFinishOpen(int dados)
	{
		return;
	}
	public delegate void Del(Animator anim, string nomeAnim);
	public void AbrirMenu(Animator anim, string nomeAnim )
	{
		anim.Play(nomeAnim);
		fundoPreto.enabled = true;
		Catapult.instance.LineRendererDisabled();
	}
	IEnumerator DelayAnim(Animator anim, string nome, float seg, Del callback)
	{
		yield return new WaitForSeconds(seg);
		callback(anim, nome);
		yield return null;
	}
}
