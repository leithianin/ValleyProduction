using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : VLY_Singleton<GameLoader>
{
    private int pendingSceneNumber;

    private void Start()
    {
        LoadSceneAdditive(1);
    }

    public static void LoadSceneAdditive(int sceneIndex)
    {
        instance.OnLoadSceneAdditive(sceneIndex);
    }

    private void OnLoadSceneAdditive(int sceneIndex)
    {
        enabled = true;

        pendingSceneNumber++;

        StartCoroutine(LoadAsyncScene(sceneIndex, LoadSceneMode.Additive));
    }

    private void OnLoadedScene()
    {
        pendingSceneNumber--;
        if(pendingSceneNumber <= 0)
        {
            enabled = false;
        }
    }

    IEnumerator LoadAsyncScene(int sceneIndex, LoadSceneMode mode)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex, mode);
        asyncLoad.allowSceneActivation = false;
        yield return (asyncLoad.progress > 0.9f);
        OnLoadedScene();
        asyncLoad.allowSceneActivation = true;
    }
}
