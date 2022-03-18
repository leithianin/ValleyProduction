using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatmapViewController : MonoBehaviour
{
    public Light mainLight;
    public Light heatmapViewLight;

    public Material[] materials;

    private bool isEnabled;

    private void Start()
    {
        EnableHeatmapView(false);

        foreach(Material m in materials)
        {
            m.SetTexture("_Mask", HeatMapMaskRenderer.maskTexture);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            HandleHeatmapNoise();
        }

        foreach (Material m in materials)
        {
            m.SetFloat("_MapSize", HeatMapMaskRenderer.staticMapSize);
        }
    }

    private void OnApplicationQuit()
    {
        EnableHeatmapView(false);
    }

    public void HandleHeatmapNoise()
    {
        EnableHeatmapView(!isEnabled);
    }

    private void EnableHeatmapView(bool enable)
    {
        isEnabled = enable;

        mainLight.enabled = !enable;
        heatmapViewLight.enabled = enable;

        
        foreach(Material m in materials)
        {
            if (enable) m.EnableKeyword("RENDER_HEATMAP");
            else m.DisableKeyword("RENDER_HEATMAP");
        }
    }
}
