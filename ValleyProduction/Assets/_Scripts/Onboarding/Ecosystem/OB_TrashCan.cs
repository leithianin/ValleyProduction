using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OB_TrashCan : OnBoarding
{
    protected override void OnEnd()
    {
       
    }

    protected override void OnPlay()
    {
        OnBoardingManager.onDestroyPath += OnClick;
    }

    public void OnClick(bool condition)
    {
        OnBoardingManager.onDestroyPath -= OnClick;
        Over();
    }
}
