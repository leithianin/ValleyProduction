using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class OB_FinishPath : OnBoarding
{
    public UnityEvent OnFinishPath;
    public UnityEvent WhilePlayerDontClickOnHiker;

    protected override void OnEnd()
    {
       
    }

    protected override void OnPlay()
    {
        PathManager.isOnFinishPath += OnFinish;
    }

    public void OnFinish(bool pathCondition)
    {
        OnFinishPath?.Invoke();
        PathManager.isOnFinishPath -= OnFinish;
        VisitorManager.isOnDespawn += RespawnHiker;
    }

    public void RespawnHiker(bool despawnCondition)
    {
        if (OnBoardingManager.firstClickVisitors) {WhilePlayerDontClickOnHiker?.Invoke()     ; }
        else                                      {VisitorManager.isOnDespawn -= RespawnHiker; }

    }
}
