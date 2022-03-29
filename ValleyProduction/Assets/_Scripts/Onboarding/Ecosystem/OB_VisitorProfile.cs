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

        OnBoardingManager.OnClickVisitorEco += OnClick; 
    }

    public void OnClick(bool condition)
    {
        OnBoardingManager.OnClickVisitorEco -= OnClick;
        Over();
    }
}
