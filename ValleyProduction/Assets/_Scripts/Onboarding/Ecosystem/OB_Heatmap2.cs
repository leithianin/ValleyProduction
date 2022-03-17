using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OB_Heatmap2 : OnBoarding
{
    private bool isWaitingCloseAction = false;

    protected override void OnEnd()
    {

    }

    protected override void OnPlay()
    {
        OnBoardingManager.OnClickHeatmapNoise += OnClick;
        isWaitingCloseAction = true;
    }

    public void OnClick(bool condition)
    {
        isWaitingCloseAction = false;
        OnBoardingManager.OnClickHeatmapNoise -= OnClick;
        Over();
    }

    public void OnClickHeatmap()
    {
        if (isWaitingCloseAction)
        {
            isWaitingCloseAction = false;
            OnBoardingManager.OnClickHeatmapNoise -= OnClick;
            Over();
        }
    }
}
