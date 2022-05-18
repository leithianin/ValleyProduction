using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OnBoardingManager : VLY_Singleton<OnBoardingManager>
{
    #region To Remove 
    public static void SetCanSpawnVisitors(bool cond)
    {
        VisitorManager.SetVisitorSpawn(cond);
    }

    public static void PlayEndPathOnBoarding()
    {
        VLY_Time.SetTimeScale(1);
        //instance.endOnboarding.EndOnBoardingPath?.Invoke();
    }

    public static void ShowChapelDirection()
    {
        //instance.UI_Arrow_Chapel.SetActive(true);
        //instance.UI_ZoneChapel.SetActive(true);
    }
    #endregion
}
