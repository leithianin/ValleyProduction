using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Heatmap : MonoBehaviour
{
    [SerializeField] private HeatmapViewController heatmapViewController;

    public void HeatmapNoise()
    {
        Debug.Log("Noise");
        heatmapViewController.HandleHeatmapNoise();
    }

    public void HeatmapVisitors()
    {

    }

    public void HeatmapFauna()
    {

    }
}
