using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OnBoardingManager : VLY_Singleton<OnBoardingManager>
{
    public bool activateOnBoarding = false;
    public static Action<bool> OnWaterMill;

    public OB_Sequence sequence;

    public GameObject UI_OB_VisitorsProfileInfo;

    private void Start()
    {
        if (activateOnBoarding)
        {
            sequence.Play();
        }
    }

    public static void PlayNextEvent()
    {
        Debug.Log("Play next Event");
        //instance.increment++;
        //instance.onBoardingList[instance.increment]?.Play();

        //
    }

    public static void ShowVisitorsProfileIntro()
    {
        instance.UI_OB_VisitorsProfileInfo.SetActive(true);
    }

    public static void HideVisitorsProfileIntro()
    {
        instance.UI_OB_VisitorsProfileInfo.SetActive(false);
    }
}
