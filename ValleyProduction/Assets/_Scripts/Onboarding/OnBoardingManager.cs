using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OnBoardingManager : VLY_Singleton<OnBoardingManager>
{
    public List<Collider> touristList = new List<Collider>();

    public UnityEvent OnProfileHiker;
    public UnityEvent OnProfileTourist;
    public UnityEvent OnProfileInfrastructure;

    public UnityEvent OnEnd;
    public UnityEvent OnCinematic;

    public static bool blockPlacePathpoint = false;
    public static bool blockFinishPath = false;

    public static void SetBlockPlacePathpoint(bool cond)
    {
        blockPlacePathpoint = cond;
    }

    public static void SetBlockFinishPath(bool cond)
    {
        blockFinishPath = cond;
    }

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

    public static void SetVisitorsInteractable(bool cond)
    {
        foreach(Collider coll in instance.touristList)
        {
            coll.enabled = cond;
        }
    }

    public static void OnEndTutorial()
    {
        instance.OnEnd?.Invoke();
    }

    public static void OnPlayCinematic()
    {
        instance.OnCinematic?.Invoke();
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
