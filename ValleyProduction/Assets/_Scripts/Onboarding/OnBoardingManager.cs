using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OnBoardingManager : VLY_Singleton<OnBoardingManager>
{
    public GameObject Welcome;
    public GameObject End;

    public UnityEvent OnProfileHiker;
    public UnityEvent OnProfileTourist;

    public UnityEvent OnProfileInfrastructure;

    public UnityEvent OnEnd;

    //unity event OnCameraMove
    public static void ClickOnHiker()
    {
        if(instance != null)
        {
            instance.OnProfileHiker?.Invoke();
        }
    }

    public static void ClickOnTourist()
    {
        if (instance != null)
        {
            instance.OnProfileTourist?.Invoke();
        }
    }

    public static void OnEndTutorial()
    {
        instance.OnEnd?.Invoke();
    }

    #region To Remove 
    public static void SetCanSpawnVisitors(bool cond)
    {
        VisitorManager.SetVisitorSpawn(cond);
    }

    public static void SetTimeToNormal()
    {
        VLY_Time.SetTimeScale(1);
    }

    public static void ShowChapelDirection()
    {
        //instance.UI_Arrow_Chapel.SetActive(true);
        //instance.UI_ZoneChapel.SetActive(true);
    }
    #endregion
}
