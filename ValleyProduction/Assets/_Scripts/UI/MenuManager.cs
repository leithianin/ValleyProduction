using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : VLY_Singleton<MenuManager>
{
    bool isSceneLoading;
    int sceneToLoad = 0;

    public static void LoadScene(int sceneIndex)
    {
        Debug.Log(instance);
        instance.Play(sceneIndex);
    }

    public static void Exit()
    {
        instance.ExitGame();
    }

    public void Play(int i) //CODE REVIEW : Pour le menu de pause, ne pas donner de r�f�rence � cette fonction directement. Passer par un script (UI_PauseMenu)
    {
        sceneToLoad = i;
        if (!isSceneLoading)
        {
            Debug.Log("Load scene");
            isSceneLoading = true;
            StartCoroutine(LoadYourAsyncScene());
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        isSceneLoading = false;
    }
}
