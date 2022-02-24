using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OB_Heatmap1 : OnBoarding
{
    //Cet Onboarding doit aussi gérer le dézoom de la camera pour montrer toute la zone
    public bool isClickOnHeatmap = false;

    public UnityEvent OnClickHeatmapEvent;

    protected override void OnPlay()
    {
        OnBoardingManager.OnClickHeatmapNoise += OnClick;
    }

    protected override void OnEnd()
    {

    }

    public void OnClick(bool condition)
    {
        OnBoardingManager.OnClickHeatmapNoise -= OnClick;
        Over();
    }

    public void OnClickHeatmap()
    {
        if(!isClickOnHeatmap)
        {
            isClickOnHeatmap = true;
            OnClickHeatmapEvent?.Invoke();
        }
    }
}
