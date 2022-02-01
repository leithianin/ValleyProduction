using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OB_VisitorsProfilesIntro : OnBoarding
{
    public bool playOnce = false;

    protected override void OnEnd()
    {
        
    }

    protected override void OnPlay()
    {
        OnBoardingManager.ShowVisitorsProfileIntro();
    }

    public void OnVisitorReach()
    {
        if (!playOnce)
        {
            playOnce = true;
            OnPlay();
        }
    }
}
