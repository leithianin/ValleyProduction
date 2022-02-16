using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OB_Heatmap2 : OnBoarding
{
    private bool isFirstClick = false;

    protected override void OnEnd()
    {

    }

    protected override void OnPlay()
    {
        OnBoardingManager.OnClickHeatmapNoise += OnClick;
    }

    public void OnClick(bool condition)
    {
        OnBoardingManager.OnClickHeatmapNoise -= OnClick;
        Over();
    }
}
