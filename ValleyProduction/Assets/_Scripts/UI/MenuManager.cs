using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : VLY_Singleton<MenuManager>
{
    bool isSceneLoading;
    int sceneToLoad = -1;
    public static int GetSceneToLoad => instance.sceneToLoad;

    public GameObject blackScreen;

    [SerializeField] private UnityEvent OnStartLoad;
    [SerializeField] private UnityEvent OnEndLoad;

    public static void LoadScene(int sceneIndex)
    {
        if(sceneIndex >= 0) 
        {
            instance.sceneToLoad = sceneIndex;
        }

        if (instance.sceneToLoad >= 0)
        {
            if (instance.blackScreen != null)
            {
                instance.blackScreen.SetActive(true);
            }
            Debug.Log(instance.sceneToLoad);
            instance.Play(instance.sceneToLoad);
        }
    }

    public static void Exit()
    {
        instance.ExitGame();
    }

    public void SetSceneIndex(int i)
    {
        if (sceneToLoad == i)
        {
            sceneToLoad = -1;
        }
        else
        {
            sceneToLoad = i;
        }
        //LoadScene(sceneToLoad);
    }

    public void Play(int i) //CODE REVIEW : Pour le menu de pause, ne pas donner de référence à cette fonction directement. Passer par un script (UI_PauseMenu)
    {
        if (sceneToLoad == -1)
        {
            sceneToLoad = i;
        }

        if (!isSceneLoading)
        {
            Debug.Log("Load scene");
            isSceneLoading = true;

            OnStartLoad?.Invoke();

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
