using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UI_VisitorInformation : MonoBehaviour
{
    private GameObject currentVisitor;

    [SerializeField] private TouristType hikersInfo;
    [SerializeField] private TouristType touristInfo;

    public Action OnShowVisitorInfo;
    public UnityEvent<GameObject> OnShow;

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
        //Pollution
        //Noise
        UI_visitorsInfo.pollution.fillAmount = visitorScript.GetThrowRadius / 10;
        UI_visitorsInfo.pollutionText.text = visitorScript.GetThrowRadius.ToString();

        //Noise
        UI_visitorsInfo.noise.fillAmount = visitorScript.noiseMade / 10;
        UI_visitorsInfo.noiseText.text = visitorScript.noiseMade.ToString();

        //Stamina
        UI_visitorsInfo.stamina.fillAmount = visitorScript.GetMaxStamina / 100;
        UI_visitorsInfo.staminaText.text = visitorScript.GetMaxStamina.ToString();
    }
}
