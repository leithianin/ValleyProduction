using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OnBoardingManager : VLY_Singleton<OnBoardingManager>
{
    public List<Collider> touristList = new List<Collider>();    //Use in Tutorial Base
    public List<Collider> hikerList = new List<Collider>();      //Use in Tutorial Advance

    public UnityEvent OnProfileHiker;
    public UnityEvent OnProfileTourist;
    public UnityEvent OnProfileInfrastructure;

    public UnityEvent OnEnd;
    public UnityEvent OnEndCinematicEvent;

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

    public static void SetHikersInteractable(bool cond)
    {
        foreach (Collider coll in instance.hikerList)
        {
            coll.enabled = cond;
        }
    }

    public static void SetAllVisitorsInteractable(bool cond)
    {
        foreach (Collider coll in instance.touristList)
        {
            coll.enabled = cond;
        }

        foreach (Collider coll in instance.hikerList)
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
        CameraManager.OnEndCinematic += OnEndCinematic;
    }
    public static void OnEndCinematic()
    {
        CameraManager.OnEndCinematic -= OnEndCinematic;
        instance.OnEndCinematicEvent?.Invoke();
    }

    public static void BlockCameraInput(bool cond)
    {
        PlayerInputManager.isCameraBlock = cond;
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
