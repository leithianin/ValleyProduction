using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OB_Noise : OnBoarding
{
    protected override void OnEnd()
    {
        
    }

    protected override void OnPlay()
    {
        OnBoardingManager.onHideVisitorInfo += OnClick;
    }

    public void OnClick(bool condition)
    {
        OnBoardingManager.onHideVisitorInfo -= OnClick;
        Over();
    }
}
