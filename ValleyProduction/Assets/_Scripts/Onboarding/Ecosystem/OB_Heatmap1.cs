using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OB_Heatmap1 : OnBoarding
{
    //Cet Onboarding doit aussi gérer le dézoom de la camera pour montrer toute la zone

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
}
