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
        Debug.Log("1");
    }

    public void OnClickInZone()
    {
        Debug.Log("2");
        Over();
    }
}
