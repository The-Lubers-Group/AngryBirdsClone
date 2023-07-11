using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneManagement : MonoBehaviour
{
    public static string previusSceneName;
	public static string mainMenu = "MenuPrincipal";
	public static string menuFases = "MenuFases";

	public GameObject objectToEnabledWhenSceneAsync;

	public void GoToMainMenu()
	{
		GoToScene(mainMenu);
	}
    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
	public void GoToMenuFases()
	{
		GoToScene(menuFases);
	}
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SetPreviousScene(string name)
    {
        previusSceneName = name;
    }

    public string GetPreviousScene()
    {
        return previusSceneName;
    }

    public void GoBack()
    {
        SceneManager.LoadScene(GetPreviousScene());
    }
	public void GoToSceneAsync(string sceneName)
	{
		StartCoroutine(LoadGameProg(sceneName));
	}
	
	IEnumerator LoadGameProg(string scene)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(scene);
        while (!async.isDone)
        {
			if(objectToEnabledWhenSceneAsync != null)
			{
				objectToEnabledWhenSceneAsync.gameObject.SetActive(true);
			}	
            yield return null;
        }
    }
}
