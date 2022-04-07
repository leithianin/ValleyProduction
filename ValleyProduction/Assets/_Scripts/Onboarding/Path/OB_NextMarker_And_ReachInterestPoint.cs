using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OB_NextMarker_And_ReachInterestPoint : OnBoarding
{
    public UnityEvent OnInterestPointReach;
    public UnityEvent OnClick;

    protected override void OnEnd()
    {
        
    }

    protected override void OnPlay()
    {
        
    }

    public void OnReach()
    {
        OnInterestPointReach?.Invoke();
        
    }

    public void OnClickButton()
    {
        OnClick?.Invoke();
    }
}
