using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatmapViewController : VLY_Singleton<HeatmapViewController>
{
    [SerializeField] private MaskRenderer msk;

    public Material[] Materials;

    public Texture defaultHeatmapTex;

    public GameObject baseLights;
    public GameObject heatmapLight;
    public GameObject foliage;

    private bool isEnabled;
    private int currentIndex;

    #region Animatic
    public bool enableHeatview;
    [Range(0, 4)] public int viewIndex;
    #endregion

    void Start()
    {
        foreach (Material m in Materials)
        {
            m.SetTexture("_DefaultTex", defaultHeatmapTex);
            m.SetTexture("_NoiseTex", msk.noiseTexture);
            m.SetTexture("_PollutionTex", msk.pollutionTexture);
            m.SetTexture("_FaunaTex", msk.faunaTexture);
            m.SetTexture("_FloraTex", msk.floraTexture);

            m.SetFloat("_MapSize", msk.MapSize);
        }

        EnableHeatmapView(false, 0);
    }

    private void OnDestroy()
    {
        HandleHeatmap(0);
    }

    private void OnApplicationQuit()
    {
        EnableHeatmapView(false, 0);
    }

    public static void HandleHeatmap(int index)
    {
        if(instance.currentIndex != index && index != 0)
        {
            instance.currentIndex = index;
            instance.EnableHeatmapView(true, index);
        }
        else
        {
            instance.currentIndex = 0;
            instance.EnableHeatmapView(false, 0);
        }
    }

    private void EnableHeatmapView(bool enable, int index)
    {
        isEnabled = enable;

        baseLights.SetActive(!enable);
        if (heatmapLight != null)
        {
            heatmapLight.SetActive(enable);
        }
        if (foliage != null)
        {
            foliage.SetActive(!enable);
        }

        foreach (Material m in Materials)
        {
            if (enable) m.EnableKeyword("RENDER_HEATMAP");
            else m.DisableKeyword("RENDER_HEATMAP");

            m.SetFloat("HEATMAP_INDEX", index);
        }
    }
}
