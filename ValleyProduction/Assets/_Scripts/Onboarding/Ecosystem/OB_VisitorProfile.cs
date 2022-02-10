using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OB_VisitorProfile : OnBoarding
{
    protected override void OnEnd()
    {
        
    }

    protected override void OnPlay()
    {
        OnBoardingManager.OnClickVisitor += OnClick; 
    }

    public void OnClick(bool condition)
    {
        OnBoardingManager.OnClickVisitor -= OnClick;
        Over();
    }
}
