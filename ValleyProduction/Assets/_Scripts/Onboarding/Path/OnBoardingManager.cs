using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OnBoardingManager : VLY_Singleton<OnBoardingManager>
{
    public bool activateOnBoarding = false;
    public bool canSpawnVisitors = true;

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
    public static Action<bool> onClickPath;
    public static Action<bool> onHideVisitorInfo;
    public static Action<bool> onDestroyPath;

    private void Start()
    {
        if(activateOnBoarding)
        {
            sequence.Play();
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
        instance.canSpawnVisitors = cond;
    }

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

    public static void DesactivateTool()
    {
        InfrastructureManager.instance.toolSelected = ToolType.None;
        ConstructionManager.SelectInfrastructureType(InfrastructureType.PathTools);
    }
    #endregion

    #region Ecosystem
    public void OnHeatmapNoise()
    {
        OnClickHeatmapNoise?.Invoke(true);
    }
    #endregion
}
