using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class OB_Tools : OnBoarding
{
    private bool isFirstClick = false;
    protected override void OnPlay()
    {
        
    }

    protected override void OnEnd()
    {
        
    }

    public void OnClick()
    {
        if (!isFirstClick)
        {
            isFirstClick = true;
            Over();
        }
    }
}
