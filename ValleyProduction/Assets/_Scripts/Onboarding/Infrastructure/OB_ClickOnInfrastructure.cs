using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OB_ClickOnInfrastructure : OnBoarding
{
    protected override void OnEnd()
    {
        
    }

    protected override void OnPlay()
    {
        OnBoardingManager.OnClickInfrastructure += OnClickInfrastructure;
    }

    public void OnClickInfrastructure(bool cond)
    {
        OnBoardingManager.OnClickInfrastructure -= OnClickInfrastructure;
        Over();
    }
}
