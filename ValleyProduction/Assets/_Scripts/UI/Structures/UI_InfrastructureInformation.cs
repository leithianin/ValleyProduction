using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_InfrastructureInformation : MonoBehaviour
{
    [SerializeField] private TMP_Text nameDisplay;
    [SerializeField] private TMP_Text descriptionDisplay;
    [SerializeField] private TMP_Text noiseScoreDisplay;
    [SerializeField] private TMP_Text pollutionScoreDisplay;
    [SerializeField] private Image imageComponent;

    public void ShowStructureInformation(ECO_AGT_Informations infoInfra, IST_BaseStructure baseStruct)
    {
        if (baseStruct.Data.Name != null)        { nameDisplay.text = baseStruct.Data.Name;}
        else { Debug.LogError("Data Name de la structure non rempli"); }

        if (baseStruct.Data.Description != null) { descriptionDisplay.text = baseStruct.Data.Description; }
        else { Debug.LogError("Data Description de la structure non rempli"); }

        if (baseStruct.Data.Logo != null)        { imageComponent.sprite = baseStruct.Data.Logo; }
        else { Debug.LogError("Data Sprite de la structure non rempli"); }

        pollutionScoreDisplay.text = infoInfra.GetPolluterScore().ToString();
        noiseScoreDisplay.text = infoInfra.GetNoiseScore().ToString();

        gameObject.SetActive(true);
    }
}
