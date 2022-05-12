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

    public TouristType currentTourist;
    public CPN_Informations currentInfo;

    [Header("Color Gradient")]
    public Gradient colorBackground;
    public static Gradient GetColorBackground;
    public Gradient colorLogo;
    public static Gradient GetColorLogo;

    public UnityEvent<GameObject> OnShow;
    public UnityEvent<GameObject> OnHide;

    private void Start()
    {
        GetColorBackground = colorBackground;
        GetColorLogo = colorLogo;
    }

    public TouristType ShowInfoVisitor(CPN_Informations cpn_Inf)
    {
        currentVisitor = cpn_Inf.gameObject;
        currentInfo = cpn_Inf;
        OnBoardingManager.OnClickVisitorEco?.Invoke(true);
        OnShow?.Invoke(currentVisitor);
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
                currentTourist = hikersInfo;
                hikersInfo.gameObject.SetActive(true);
                return hikersInfo;
            case TypeVisitor.Tourist:
                ChangeInfoVisitor(touristInfo, cpn_Inf);
                currentTourist = touristInfo;
                touristInfo.gameObject.SetActive(true);
                return touristInfo;
        }
        return null;
    }

    public void HideVisitorInformation()
    {
        OnHide?.Invoke(currentVisitor);
        UIManager.HideShownGameObject();
    }

    public static void ChangeInfoVisitor(TouristType UI_visitorsInfo, CPN_Informations cpn_Inf)
    {
        
        UI_visitorsInfo.name.text = cpn_Inf.GetName;
        VisitorScriptable visitorScript = cpn_Inf.scriptable;

        ChangeNoise(UI_visitorsInfo, visitorScript);
        ChangePollution(UI_visitorsInfo, visitorScript);
        ChangeStamina(UI_visitorsInfo, cpn_Inf.components.GetComponentOfType<CPN_Stamina>());
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

    public static void ChangeStamina(TouristType UI_visitorsInfo, CPN_Stamina cpn_stamina)
    {
        float currentStamina = cpn_stamina.GetStamina/10;

        for(int i = 0; i < UI_visitorsInfo.staminaLevel.Count; i++ )
        {
            if (currentStamina > 1f)
            {
                UI_visitorsInfo.staminaLevel[i].fillAmount = 1f;
                currentStamina -= 1f;
            }
            else
            {
                UI_visitorsInfo.staminaLevel[i].fillAmount = currentStamina;
                currentStamina = 0f;
            }
        }
    }

    public void UpdateNoise()
    {

    }

    public void UpdatePollution()
    {

    }

    public void UpdateStamina()
    {
        ChangeStamina(currentTourist, currentInfo.components.GetComponentOfType<CPN_Stamina>());
    }

    public void ResetSavedVisitors()
    {
        currentInfo = null;
        currentTourist = null;
        currentVisitor = null;
    }
}
