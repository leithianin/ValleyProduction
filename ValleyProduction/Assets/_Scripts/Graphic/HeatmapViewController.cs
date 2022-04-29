using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatmapViewController : MonoBehaviour
{
    public Material[] Materials;

    [SerializeField] private MaskRenderer msk;

    private bool isEnabled;

    void Start()
    {
        foreach (Material m in Materials)
        {
            m.SetTexture("_NoiseTex", msk.noiseTexture);
            m.SetTexture("_PollutionTex", msk.pollutionTexture);
            m.SetTexture("_FaunaTex", msk.faunaTexture);
            m.SetTexture("_FloraTex", msk.floraTexture);
        }

        EnableHeatmapView(false, 0);
    }

    void Update()
    {
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

        foreach (Material m in Materials)
        {
            m.SetFloat("_MapSize", msk.MapSize);
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

        foreach (Material m in Materials)
        {
            if (enable) m.EnableKeyword("RENDER_HEATMAP");
            else m.DisableKeyword("RENDER_HEATMAP");

            m.SetFloat("HEATMAP_INDEX", index);
        }
    }
}
