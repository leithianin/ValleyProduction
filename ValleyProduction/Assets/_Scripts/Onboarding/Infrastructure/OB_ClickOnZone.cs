using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OB_ClickOnZone : OnBoarding
{
    protected override void OnEnd()
    {
        
    }

    protected override void OnPlay()
    {
        OnBoardingManager.OnClickZone += ClickOnZone;
    }

    public void ClickOnZone(bool cond)
    {
        OnBoardingManager.OnClickZone -= ClickOnZone;
        Over();
    }
}
