using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OB_OnReachChapel : OnBoarding
{
    protected override void OnEnd()
    {
       
    }

    protected override void OnPlay()
    {
        OnBoardingManager.OnChapel += OnReachChapel;
    }

    public void OnReachChapel(bool condition)
    {
        OnBoardingManager.OnChapel -= OnReachChapel;
        Over();
    }
}
