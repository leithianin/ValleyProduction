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
        if(ConstructionManager.HasSelectedStructureType) { UIManager.UIinstance.OnToolCreatePath(0); }
        OnBoardingManager.ShowVisitorsProfileIntro();
        OnBoardingManager.SetCanSpawnVisitors(true);
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
