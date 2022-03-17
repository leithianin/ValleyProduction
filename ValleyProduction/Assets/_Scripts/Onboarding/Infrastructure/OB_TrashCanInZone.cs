using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OB_TrashCanInZone : OnBoarding
{
    protected override void OnEnd()
    {
        
    }

    protected override void OnPlay()
    {
        OnBoardingManager.OnClickZoneTrashCan += OnClickInZone;
    }

    public void OnClickInZone(bool cond)
    {
        OnBoardingManager.OnClickZoneTrashCan -= OnClickInZone;
        Over();
    }
}
