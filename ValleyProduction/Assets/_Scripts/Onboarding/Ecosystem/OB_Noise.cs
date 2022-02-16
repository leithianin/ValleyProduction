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
        Debug.Log("Wait hide info");
        OnBoardingManager.onHideVisitorInfo += OnClick;
    }

    public void OnClick(bool condition)
    {
        Debug.Log("Yes hide info");
        OnBoardingManager.onHideVisitorInfo -= OnClick;
        Over();
    }
}
