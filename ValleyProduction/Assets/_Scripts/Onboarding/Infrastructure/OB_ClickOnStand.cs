using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OB_ClickOnStand : OnBoarding
{
    protected override void OnEnd()
    {
        
    }

    protected override void OnPlay()
    {
        OnBoardingManager.OnClickFoodInfrastructure += OnClickFoodInfrastruture;
    }

    public void OnClickFoodInfrastruture(bool cond)
    {
        OnBoardingManager.OnClickFoodInfrastructure -= OnClickFoodInfrastruture;
        Over();
    }
}
