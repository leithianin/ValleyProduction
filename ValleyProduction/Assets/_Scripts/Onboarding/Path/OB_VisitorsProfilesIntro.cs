using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OB_VisitorsProfilesIntro : OnBoarding
{
    private bool playOnce = false;

    [Header("This events are activate in OnBoardingManager")]
    public UnityEvent OnShowInfo;
    public UnityEvent OnReach;

    protected override void OnEnd()
    {
        
    }

    protected override void OnPlay()
    {
        
        if (ConstructionManager.HasSelectedStructureType) { UIManager.UIinstance.OnToolCreatePath(0); }
        OnBoardingManager.ShowVisitorsProfileIntro();
        OnBoardingManager.SetCanSpawnVisitors(true);
        OnBoardingManager.firstClickVisitors = true;
        Over();
    }

    public void PlayEvent()
    {
        
        OnShowInfo?.Invoke();
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
