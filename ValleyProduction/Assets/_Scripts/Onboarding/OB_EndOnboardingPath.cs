using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OB_EndOnboardingPath : OnBoarding
{
    public UnityEvent EndOnBoardingPath;

    protected override void OnEnd()
    {
        
    }

    protected override void OnPlay()
    {
        
    }

    public void Play()
    {

        EndOnBoardingPath?.Invoke();
    }
}
