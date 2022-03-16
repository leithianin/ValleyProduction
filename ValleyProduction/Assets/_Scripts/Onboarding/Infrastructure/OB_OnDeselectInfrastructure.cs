using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OB_OnDeselectInfrastructure : OnBoarding
{
    protected override void OnEnd()
    {
        
    }

    protected override void OnPlay()
    {
        OnBoardingManager.OnDeselectInfrastructure += OnDeselectInfrastructure;
    }

    public void OnDeselectInfrastructure(bool cond)
    {
        OnBoardingManager.OnDeselectInfrastructure -= OnDeselectInfrastructure;
        Over();
    }
}
