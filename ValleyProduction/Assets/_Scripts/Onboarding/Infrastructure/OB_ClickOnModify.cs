using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OB_ClickOnModify : OnBoarding
{
    protected override void OnEnd()
    {
        
    }

    protected override void OnPlay()
    {
        OnBoardingManager.OnClickModify += OnClickModify;
    }

    public void OnClickModify(bool cond)
    {
        OnBoardingManager.OnClickModify -= OnClickModify;
        Over();
    }
}
