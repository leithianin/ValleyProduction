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

    public UnityEvent OnStart;
    public UnityEvent OnDisableEvent;

    public UnityEvent OnProfileHiker;
    public UnityEvent OnProfileTourist;
    public UnityEvent OnProfileInfrastructure;

    public UnityEvent OnEnd;
    public UnityEvent OnEndWelcomeCinematicEvent;
    public UnityEvent OnEndEndCinematicEvent;

    public TextDialogue dialogueWelcome;
    public TextDialogue dialogueEnd;


    public static bool blockPlacePathpoint = false;
    public static bool blockFinishPath = false;
    private void OnDisable()
    {
        OnDisableEvent?.Invoke();
        blockPlacePathpoint = false;
        blockFinishPath = false;
    }

    private void Start()
    {
        TimerManager.CreateRealTimer(4f, () => OnStart?.Invoke());
    }

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

    public static void OnPlayWelcomeCinematic()
    {
        CameraManager.OnEndCinematic += instance.OnEndWelcomeCinematic;
    }
    private void OnEndWelcomeCinematic()
    {
        CameraManager.OnEndCinematic -= OnEndWelcomeCinematic;
        instance.OnEndWelcomeCinematicEvent?.Invoke();
    }

    public static void OnPlayEndCinematic()
    {
        CameraManager.OnEndCinematic += instance.OnEndEndCinematic;
    }
    private void OnEndEndCinematic()
    {
        CameraManager.OnEndCinematic -= OnEndEndCinematic;
        instance.OnEndEndCinematicEvent?.Invoke();
    }

    public static void BlockCameraInput(bool cond)
    {
        PlayerInputManager.EnableOrDisableCameraControl(cond);
    }

    #region To Remove 
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
