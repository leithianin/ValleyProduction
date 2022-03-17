using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OB_OnClickTrashCan : OnBoarding
{
    private bool clickTrashCan;

    protected override void OnEnd()
    {
        
    }

    protected override void OnPlay()
    {
        
    }

    public void OnClickTrashCan(bool cond)
    {
        if(clickTrashCan)
        {
            clickTrashCan = false;
            Over();
        }
    }
}
