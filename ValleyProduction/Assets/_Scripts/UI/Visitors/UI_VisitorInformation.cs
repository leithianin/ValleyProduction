using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_VisitorInformation : MonoBehaviour
{
    private GameObject currentVisitor;

    [SerializeField] private TouristType touristInfo;
    [SerializeField] private TouristType hikersInfo;

    [Header("Color Gradient")]
    public Gradient colorBackground;
    public static Gradient GetColorBackground;
    public Gradient colorLogo;
    public static Gradient GetColorLogo;

    public Action OnShowVisitorInfo;
    public UnityEvent<GameObject> OnShow;

    private void Start()
    {
        GetColorBackground = colorBackground;
        GetColorLogo = colorLogo;
    }

    public TouristType ShowInfoVisitor(CPN_Informations cpn_Inf)
    {
        currentVisitor = cpn_Inf.gameObject;
        OnBoardingManager.OnClickVisitorEco?.Invoke(true);
        OnShow?.Invoke(currentVisitor);
        OnShowVisitorInfo?.Invoke();
        switch (cpn_Inf.visitorType)
        {
            case TypeVisitor.Hiker:
                if (OnBoardingManager.firstClickVisitors)
                {
                    OnBoardingManager.OnClickVisitorPath?.Invoke(true);
                    OnBoardingManager.ShowHikerProfileIntro();
                    OnBoardingManager.firstClickVisitors = false;
                }
                ChangeInfoVisitor(hikersInfo, cpn_Inf);
                hikersInfo.gameObject.SetActive(true);
                return hikersInfo;
            case TypeVisitor.Tourist:
                ChangeInfoVisitor(touristInfo, cpn_Inf);
                touristInfo.gameObject.SetActive(true);
                return touristInfo;
        }
        return null;
    }

    public static void ChangeInfoVisitor(TouristType UI_visitorsInfo, CPN_Informations cpn_Inf)
    {
        UI_visitorsInfo.name.text = cpn_Inf.GetName;
        VisitorScriptable visitorScript = cpn_Inf.scriptable;

        ChangeNoise(UI_visitorsInfo, visitorScript);
        ChangePollution(UI_visitorsInfo, visitorScript);

        //Stamina
        /*
        UI_visitorsInfo.stamina.fillAmount = visitorScript.GetMaxStamina / 100;
        UI_visitorsInfo.staminaText.text = visitorScript.GetMaxStamina.ToString();*/
    }

    public static void ChangeNoise(TouristType UI_visitorsInfo, VisitorScriptable visitorScript)
    {
        foreach(TextMeshProUGUI tmp in UI_visitorsInfo.noiseText)
        {
            tmp.text = visitorScript.noiseMade.ToString();
        }

        foreach (Image image in UI_visitorsInfo.noiseListBackground)
        {
            image.color = GetColorBackground.Evaluate(visitorScript.noiseMade/10f);
        }

        foreach (Image image in UI_visitorsInfo.noiseListImage)
        {
            image.color = GetColorLogo.Evaluate(visitorScript.noiseMade/10f);
        }
    }

    public static void ChangePollution(TouristType UI_visitorsInfo, VisitorScriptable visitorScript)
    {
        foreach (TextMeshProUGUI tmp in UI_visitorsInfo.pollutionText)
        {
            tmp.text = visitorScript.GetThrowRadius.ToString();
        }

        foreach (Image image in UI_visitorsInfo.pollutionListBackground)
        {
            image.color = GetColorBackground.Evaluate(visitorScript.GetThrowRadius/10f);
        }

        foreach (Image image in UI_visitorsInfo.pollutionListImage)
        {
            image.color = GetColorLogo.Evaluate(visitorScript.GetThrowRadius/10f);
        }
    }
}
