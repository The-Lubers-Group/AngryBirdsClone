using TMPro;
using UnityEngine;
using UnityEngine.UI;
public enum UsableObjectStatus
{
	Inactive, Active
}
public class UsableObject : MonoBehaviour
{
	TextMeshProUGUI _textQtdRestante;
	[SerializeField] private string _keyName;
	[SerializeField] private UsableObjectStatus _status;
	public UsableObjectsEnum objectType;
	[SerializeField] private Sprite _objectInactiveImg;
	[SerializeField] private Sprite _objectActiveImg;
	private int qtdObj;
	private Image _spriteRender;

	[ContextMenu("carregar unidades")]
	void cargarUnidades()
	{
		qtdObj = 6;
		ZPlayerPrefs.SetInt(_keyName, 6);
		UpdateObjectText(qtdObj);
	}
    void Awake()
    {
        _textQtdRestante = GetComponentInChildren<TextMeshProUGUI>();
		_spriteRender = GetComponentInChildren<Image>();
		_status = UsableObjectStatus.Inactive;
		_spriteRender.sprite = _objectInactiveImg;
		LoadObject();
    }
	//no start fazemos que ele veja se o objeto já estava ativo, e ajustamos internamente o status simulando um click
	private void Start()
	{
		if ( GameManager.instance.mira )
		{
			_status = UsableObjectStatus.Inactive;
		}
		else
		{
			_status = UsableObjectStatus.Active;
		}
		OnClick();
	}
	
	// Carrega a quantidade de objetos dessa key na memoria
	void LoadObject()
	{
		if (ZPlayerPrefs.HasKey(_keyName))
		{
			qtdObj = ZPlayerPrefs.GetInt(_keyName);
		}
		else
		{
			qtdObj = 0;
		}
		_textQtdRestante.text = qtdObj.ToString();
	}
	// metodo usado pelo botão para ativar ou desativar o objeto
	public void OnClick() 
	{
		if(_status == UsableObjectStatus.Inactive && qtdObj > 0)
		{
			SetStatusObject(UsableObjectStatus.Active);
		}
		else
		{
			SetStatusObject(UsableObjectStatus.Inactive);
		}
	}

	//Método interno para realizar determinadas ações segundo o status passado
    private void SetStatusObject(UsableObjectStatus status)
	{
		switch (status) 
		{
			case UsableObjectStatus.Active:
				_status = status;
				UsableObjectsManager.instance.SetActiveObject(this.objectType);
				_spriteRender.sprite = _objectActiveImg;
				break;
			
			case UsableObjectStatus.Inactive:
				_status = status;
				UsableObjectsManager.instance.SetInactiveObject(this.objectType);
				_spriteRender.sprite = _objectInactiveImg;
				break;
		}
	}
	//Atualiza o valor do texto da quantidade de objetos restantes
	public bool UpdateObjectText(int n)
	{
		if (ZPlayerPrefs.HasKey(_keyName))
		{ 
			qtdObj = n;
		
			if (qtdObj > 0)
			{
				ZPlayerPrefs.SetInt(_keyName, n);
				_textQtdRestante.text = qtdObj.ToString();
				return true;
			} 
			ZPlayerPrefs.SetInt(_keyName, 0);
			qtdObj = 0;
			_textQtdRestante.text = "0";
			SetStatusObject(UsableObjectStatus.Inactive);
			return false;
		}
		else
		{
			ZPlayerPrefs.SetInt(_keyName, 0);
			qtdObj = 0;
			_textQtdRestante.text = "0";
			SetStatusObject(UsableObjectStatus.Inactive);
			return false;
		}
	}
	// Retira uma unidade do objeto
	public bool UseObject()
	{
		--qtdObj;
		return UpdateObjectText(qtdObj); 
	}

	//Adiciona 1 ou mas unidades ao objeto
	public void GiveObject(int n=1 )
	{
		UpdateObjectText(qtdObj + n);
	}
}
