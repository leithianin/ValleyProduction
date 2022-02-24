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
        Debug.Log("test1");
        OnBoardingManager.OnClickVisitorEco += OnClick; 
    }

    public void OnClick(bool condition)
    {
        Debug.Log("test2");
        OnBoardingManager.OnClickVisitorEco -= OnClick;
        Over();
    }
}
