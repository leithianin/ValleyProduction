using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UI_InfrastructureInformation : MonoBehaviour
{
    [SerializeField] private TMP_Text nameDisplay;
    [SerializeField] private TMP_Text descriptionDisplay;
    [SerializeField] private TMP_Text noiseScoreDisplay;
    [SerializeField] private TMP_Text pollutionScoreDisplay;
    [SerializeField] private Image imageComponent;

    [Header("Feature Icons")]
    [SerializeField] private GameObject capacity;
    [SerializeField] private TMP_Text capacityText;
    [SerializeField] private GameObject visitorsTotal;
    [SerializeField] private TMP_Text visitorsTotalText;
    [SerializeField] private GameObject moneyTotal;
    [SerializeField] private TMP_Text moneyTotalText;

    public Infrastructure openedInfrastructure;                                         //Infrastrucure dont l'UI est actuellement ouverte


    public void ShowStructureInformation(ECO_AGT_Informations infoInfra, Infrastructure baseStruct)
    {
        openedInfrastructure = baseStruct;

        if (baseStruct.infraDataRunTime.name != string.Empty)
        {
            nameDisplay.text = baseStruct.infraDataRunTime.name;
        }
        else
        {
            nameDisplay.text = baseStruct.Data.Name;
        }

        if (baseStruct.Data.Description != null) { descriptionDisplay.text = baseStruct.Data.Description; }
        else { Debug.LogError("Data Description de la structure non rempli"); }

        
        if (baseStruct.Data.Logo != null)        { imageComponent.sprite = baseStruct.Data.Logo; }
        else { Debug.LogError("Data Sprite de la structure non rempli"); }

        //Show Capacity si interactionScript
        if(baseStruct.interestPoint != null) 
        {
            Debug.Log(baseStruct.interestPoint);
            if (capacityText != null)
            {
                capacityText.text = "Capacity : <size=17>" + baseStruct.interestPoint.GetCurrentNbVisitors().ToString() + "/" + baseStruct.interestPoint.GetInteractionMaxVisitors().ToString();
            }

            if (capacity != null)
            {
                capacity.SetActive(true);
            }
        }

        //Show visitorsTotaux
        visitorsTotalText.text = baseStruct.infraDataRunTime.visitorsTotal.ToString();
        visitorsTotal.SetActive(true);

        //Show Money
        moneyTotalText.text = baseStruct.infraDataRunTime.moneyTotal.ToString();
        moneyTotal.SetActive(true);

        pollutionScoreDisplay.text = "Pollution : <size=17>" + infoInfra.GetPolluterScore().ToString() + "</size>";
        noiseScoreDisplay.text = "Noise : <size=17>" + infoInfra.GetNoiseScore().ToString() + "</size>";

        Debug.Log("Test");

        gameObject.SetActive(true);
    }

    public void UpdateCurrentNbInfo(Infrastructure baseStruct)
    {
        capacityText.text = "Capacity : <size=17>" + baseStruct.interestPoint.GetCurrentNbVisitors().ToString() + "/" + baseStruct.interestPoint.GetInteractionMaxVisitors().ToString();
    }

    public void UpdateTotalNbInfo(Infrastructure baseStruct)
    {
        visitorsTotalText.text = baseStruct.infraDataRunTime.visitorsTotal.ToString();
    }

    public void UpdateTotalMoney(Infrastructure baseStruct)
    {
        moneyTotalText.text = baseStruct.infraDataRunTime.moneyTotal.ToString() + "€";
    }

    public void ResetSavedInfrastructe()
    {
        openedInfrastructure = null;
    }

    public void HideInfrastructureInfo()
    {
        UIManager.HideShownGameObject();
    }

    public void SetStructureOpen(bool isOpen)
    {
        if(InfrastructureManager.GetCurrentSelectedStructure != null)
        {
            if(isOpen)
            {
                InfrastructureManager.GetCurrentSelectedStructure.OpenStructure();
            }
            else
            {
                InfrastructureManager.GetCurrentSelectedStructure.CloseStructure();
            }
        }
    }
}
