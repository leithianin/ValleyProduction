using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OB_AvoidForest : OnBoarding
{
    protected override void OnEnd()
    {
        
    }

    protected override void OnPlay()
    {
        OnBoardingManager.OnWaterMill += OnClick;
    }

    public void OnClick(bool condition)
    {
        Debug.Log("WaterMill");
        OnBoardingManager.OnWaterMill -= OnClick;
        Over();
    }
}
