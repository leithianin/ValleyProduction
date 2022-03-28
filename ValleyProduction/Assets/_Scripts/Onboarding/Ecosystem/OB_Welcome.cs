using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OB_Welcome : OnBoarding
{
    public UnityEvent OnClickEvent;

    protected override void OnEnd()
    {
        
    }

    protected override void OnPlay()
    {
        
    }

    public void OnClick()
    {
        OnClickEvent?.Invoke();
    }
}
