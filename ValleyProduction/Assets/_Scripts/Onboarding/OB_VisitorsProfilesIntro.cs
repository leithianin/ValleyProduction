using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OB_VisitorsProfilesIntro : OnBoarding
{
    protected override void OnEnd()
    {
        
    }

    protected override void OnPlay()
    {
        OnBoardingManager.ShowVisitorsProfileIntro();
    }

    public void OnVisitorReach()
    {
        OnPlay();
    }
}
