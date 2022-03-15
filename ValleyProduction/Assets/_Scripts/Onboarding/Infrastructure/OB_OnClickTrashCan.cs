using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OB_OnClickTrashCan : OnBoarding
{
    protected override void OnEnd()
    {
        
    }

    protected override void OnPlay()
    {
        OnBoardingManager.OnClickTrashCan += OnClickTrashCan;
    }

    public void OnClickTrashCan(bool cond)
    {
        OnBoardingManager.OnClickTrashCan -= OnClickTrashCan;
        Over();
    }
}
