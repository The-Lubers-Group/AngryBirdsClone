using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnToUrl : MonoBehaviour
{
    public void GoToUrl(string url)
    {
        Application.OpenURL(url);
    }
}
