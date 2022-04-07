using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OB_DestroyPath : OnBoarding
{
    protected override void OnEnd()
    {
        
    }

    protected override void OnPlay()
    {
        OnBoardingManager.onClickPath += OnClick;
    }

    public void OnClick(bool condition)
    {
        OnBoardingManager.onClickPath -= OnClick;
        Over();
    }
}
