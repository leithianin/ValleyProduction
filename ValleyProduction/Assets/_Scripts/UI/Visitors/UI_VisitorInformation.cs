using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_VisitorInformation : MonoBehaviour
{
    public TouristType hikersInfo;
    public TouristType touristInfo;

    public TouristType ShowInfoVisitor(VisitorBehavior visitorInfo)
    {
        OnBoardingManager.OnClickVisitorEco?.Invoke(true);
        CPN_Informations cpn_Inf = visitorInfo.GetComponent<CPN_Informations>();
        switch (cpn_Inf.visitorType)
        {
            case TypeVisitor.Hiker:
                if (OnBoardingManager.firstClickVisitors)
                {
                    OnBoardingManager.OnClickVisitorPath?.Invoke(true);
                    OnBoardingManager.ShowHikerProfileIntro();
                    OnBoardingManager.firstClickVisitors = false;
                }
                ChangeInfoVisitor(hikersInfo, visitorInfo, cpn_Inf);
                hikersInfo.gameObject.SetActive(true);
                return hikersInfo;
            case TypeVisitor.Tourist:
                ChangeInfoVisitor(touristInfo, visitorInfo, cpn_Inf);
                touristInfo.gameObject.SetActive(true);
                return touristInfo;
        }
        return null;
    }

    public static void ChangeInfoVisitor(TouristType UI_visitorsInfo, VisitorBehavior visitorInfo, CPN_Informations cpn_Inf)
    {
        UI_visitorsInfo.name.text = cpn_Inf.GetName;
        VisitorScriptable visitorScript = visitorInfo.visitorType;
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
