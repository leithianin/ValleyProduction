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
    public GameObject tool;                                                             //CODE REVIEW
    public static Action<bool> OnWaterMill;
    public static Action<bool> OnChapel;
    public static Action<bool> OnClickVisitorPath;
    public static bool firstClickVisitors = false;
    public GameObject UI_OB_VisitorsProfileInfo;
    public GameObject UI_OB_HikerIntro;
    public GameObject UI_Arrow_Chapel;
    public GameObject UI_ZoneChapel;
    public OB_VisitorsProfilesIntro visitorProfileIntroOnBoarding;
    public OB_EndOnboardingPath endOnboarding;
    public OB_OnReachChapel chapelOnboarding;

    [Header("On Boarding Ecosystem")]
    public static Action<bool> OnClickHeatmapNoise;
    public static Action<bool> OnClickVisitorEco;
    public static Action<bool> onClickPath;
    public static Action<bool> onHideVisitorInfo;
    public static Action<bool> onDestroyPath;
    public static Action<bool> OnWindMill;
    public static Action<bool> OnStartPoint;

    [Header("On Boarding Infrastructure")]
    public static Action<bool> OnClickInfrastructure;
    public static Action<bool> OnDeselectInfrastructure;
    public static Action<bool> OnClickModify;
    public static Action<bool> OnClickFoodInfrastructure;
    public static Action<bool> OnClickZone;
    public static Action<bool> OnClickBuild;
    public static Action<bool> OnClickZoneTrashCan;

    private void Start()
    {
        if(activateOnBoarding)
        {
            sequence.Play();
        }
        else
        {
            VisitorManager.SetVisitorSpawn(true);
        }
    }

    public static void PlayNextEvent()
    {
        Debug.Log("Play next Event");
        //instance.increment++;
        //instance.onBoardingList[instance.increment]?.Play();
    }

    #region Path
    public static void SetCanSpawnVisitors(bool cond)
    {
        VisitorManager.SetVisitorSpawn(cond);
    }

    public static void PlayEndPathOnBoarding()
    {
        VLY_Time.SetTimeScale(1);
        instance.endOnboarding.EndOnBoardingPath?.Invoke();
    }

    public static void ShowVisitorsProfileIntro()
    {
        //instance.UI_OB_VisitorsProfileInfo.SetActive(true);
        instance.visitorProfileIntroOnBoarding.OnShowInfo?.Invoke();
        instance.visitorProfileIntroOnBoarding.OnReach?.Invoke();
    }

    public static void ShowHikerProfileIntro()
    {
        instance.visitorProfileIntroOnBoarding.Over();
        instance.chapelOnboarding.Play();
        instance.activateOnBoarding = false;
        //instance.UI_OB_VisitorsProfileInfo.SetActive(false);
        //instance.UI_OB_HikerIntro.SetActive(true);
    }

    public static void ShowChapelDirection()
    {
        instance.UI_Arrow_Chapel.SetActive(true);
        instance.UI_ZoneChapel.SetActive(true);
    }

    public static void DesactivateTool()
    {
        InfrastructureManager.instance.toolSelected = ToolType.None;
        ConstructionManager.SelectInfrastructureType(null);
    }
    #endregion

    #region Ecosystem
    public void OnHeatmapNoise()
    {
        OnClickHeatmapNoise?.Invoke(true);
    }
    #endregion

    #region Infrastructure
    public void OnInfraMove()
    {
        OnClickFoodInfrastructure?.Invoke(true);
    }
    #endregion
}
