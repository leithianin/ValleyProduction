using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OnBoardingManager : VLY_Singleton<OnBoardingManager>
{
    public bool activateOnBoarding = false;

    public OB_Sequence sequence;

    [Header("On Boarding Path")]
    public GameObject tool;
    public static Action<bool> OnWaterMill;
    public static Action<bool> OnClickVisitor;
    public static bool firstClickVisitors = false;
    public GameObject UI_OB_VisitorsProfileInfo;
    public GameObject UI_OB_HikerIntro;
    public OB_EndOnboardingPath endOnboarding;

    [Header("On Boarding Ecosystem")]
    public static Action<bool> OnClickHeatmapNoise;
    public static Action<bool> onClickVisitor;

    private void Start()
    {
        if (activateOnBoarding) {sequence.Play();}
        else                    {tool.SetActive(true);}
    }

    public static void PlayNextEvent()
    {
        Debug.Log("Play next Event");
        //instance.increment++;
        //instance.onBoardingList[instance.increment]?.Play();
    }

    #region Path
    public static void PlayEndPathOnBoarding()
    {
        instance.endOnboarding.EndOnBoardingPath?.Invoke();
    }

    public static void ShowVisitorsProfileIntro()
    {
        instance.UI_OB_VisitorsProfileInfo.SetActive(true);
    }

    public static void ShowHikerProfileIntro()
    {
        instance.UI_OB_VisitorsProfileInfo.SetActive(false);
        instance.UI_OB_HikerIntro.SetActive(true);
        instance.activateOnBoarding = false;
    }
    #endregion

    #region Ecosystem
    public void OnHeatmapNoise()
    {
        OnClickHeatmapNoise?.Invoke(true);
    }
    #endregion
}
