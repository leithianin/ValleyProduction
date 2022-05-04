using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotsManager : MonoBehaviour
{
    public static ScreenshotsManager instance;
    public List<RenderTexture> screenshotList = new List<RenderTexture>();

    public static ScreenshotsManager Instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
        {
            Debug.Log("Screenshot Manager put in DontDestroyOnLoad");
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }       
    }
}
