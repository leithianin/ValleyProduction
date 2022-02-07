using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OB_EndOnboardingPath : OnBoarding
{
    public UnityEvent EndOnBoardingPath;
    public bool playOnce = false;

    protected override void OnEnd()
    {

    }

    protected override void OnPlay()
    {
        OnBoardingManager.PlayEndPathOnBoarding();
    }

    public void PlayOnReach()
    {
        if (!playOnce)
        {
            playOnce = true;
            OnPlay();
        }
    }
}
