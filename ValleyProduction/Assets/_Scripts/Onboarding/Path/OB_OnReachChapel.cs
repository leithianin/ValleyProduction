using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OB_OnReachChapel : OnBoarding
{
    public UnityEvent OnClick;

    protected override void OnEnd()
    {
       
    }

    protected override void OnPlay()
    {
    }

    public void OnReachChapel()
    {
        PathManager.isOnFinishPath += OnFinishPath;
    }

    public void OnFinishPath(bool condition)
    {
        PathManager.isOnFinishPath -= OnFinishPath;
        Over();
    }

    public void OnClickButtonPTH_008()
    {
        OnClick?.Invoke();
        OnBoardingManager.ShowChapelDirection();
    }
}
