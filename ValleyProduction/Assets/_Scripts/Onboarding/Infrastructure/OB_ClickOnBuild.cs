using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OB_ClickOnBuild : OnBoarding
{
    protected override void OnEnd()
    {

    }

    protected override void OnPlay()
    {
        OnBoardingManager.OnClickBuild += OnClickBuild;
    }

    public void OnClickBuild(bool cond)
    {
        OnBoardingManager.OnClickBuild -= OnClickBuild;
        Over();
    }
}
