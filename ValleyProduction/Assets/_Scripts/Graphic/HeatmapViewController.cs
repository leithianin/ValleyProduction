using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatmapViewController : MonoBehaviour
{
    [SerializeField] private MaskRenderer msk;

    public Material[] Materials;

    public Texture defaultHeatmapTex;

    public GameObject baseLights;
    public GameObject heatmapLight;
    public GameObject foliage;

    private bool isEnabled;

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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            HandleHeatmap(0);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            HandleHeatmap(1);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            HandleHeatmap(2);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            HandleHeatmap(3);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            HandleHeatmap(4);
        }
    }

    private void OnApplicationQuit()
    {
        EnableHeatmapView(false, 0);
    }
    public void HandleHeatmap(int index)
    {
        EnableHeatmapView(!isEnabled, index);
    }

    private void EnableHeatmapView(bool enable, int index)
    {
        isEnabled = enable;

        baseLights.SetActive(!enable);
        heatmapLight.SetActive(enable);
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
