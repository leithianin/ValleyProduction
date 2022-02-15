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
        if(ConstructionManager.HasSelectedStructureType) { ConstructionManager.UnselectInfrastructureType(); }
        OnBoardingManager.ShowVisitorsProfileIntro();
        OnBoardingManager.firstClickVisitors = true;
        Over();
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
