using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Events;

public class UI_InfrastructureInformation : MonoBehaviour
{
    [SerializeField] private TMP_Text nameDisplay;
    [SerializeField] private TMP_Text descriptionDisplay;
    [SerializeField] private TMP_Text noiseScoreDisplay;
    [SerializeField] private TMP_Text pollutionScoreDisplay;
    [SerializeField] private Image imageComponent;

    [SerializeField] private Animator closeOpenAnimator;

    [Header("Feature Icons")]
    [SerializeField] private GameObject capacity;
    [SerializeField] private TMP_Text capacityText;
    [SerializeField] private GameObject visitorsTotal;
    [SerializeField] private TMP_Text visitorsTotalText;
    [SerializeField] private GameObject moneyTotal;
    [SerializeField] private TMP_Text moneyTotalText;

    [Header("Color Gradient")]
    public Gradient colorBackground;
    public Gradient GetColorBackground;
    public Gradient colorLogo;
    public Gradient GetColorLogo;

    [Header("Noise")]
    public Image noiseBackground;
    public Image noiseImage;

    [Header("Pollution")]
    public Image pollutionBackground;
    public Image pollutionImage;


    public Infrastructure openedInfrastructure;                                         //Infrastrucure dont l'UI est actuellement ouverte

    public UnityEvent<GameObject> OnShow;
    public UnityEvent<GameObject> OnHide;

    private void OnEnable()
    {
        GetColorBackground = colorBackground;
        GetColorLogo = colorLogo;
    }

    public void ShowStructureInformation(ECO_AGT_Informations infoInfra, Infrastructure baseStruct)
    {
        openedInfrastructure = baseStruct;
        OnShow?.Invoke(infoInfra.gameObject);

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

        
        if (baseStruct.Data.ButtonIcon != null)        { imageComponent.sprite = baseStruct.Data.ButtonIcon; }
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
        ShowPollution(infoInfra);
        noiseScoreDisplay.text = "Noise : <size=17>" + infoInfra.GetNoiseScore().ToString() + "</size>";
        ShowNoise(infoInfra);

        gameObject.SetActive(true);

        closeOpenAnimator.SetBool("Selected", !baseStruct.IsOpen);
    }

    public void ShowNoise(ECO_AGT_Informations infoInfra)
    {
        noiseBackground.color = colorBackground.Evaluate(infoInfra.GetNoiseScore() / 10f);
        noiseImage.color = colorLogo.Evaluate(infoInfra.GetNoiseScore() / 10f);
    }

    public void ShowPollution(ECO_AGT_Informations infoInfra)
    {
        pollutionBackground.color = colorBackground.Evaluate(infoInfra.GetPolluterScore() / 10f);
        pollutionImage.color = colorLogo.Evaluate(infoInfra.GetPolluterScore() / 10f);
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

    public void OnHideFunction()
    {
        if (openedInfrastructure != null)
        {
            OnHide?.Invoke(openedInfrastructure.gameObject);
            ResetSavedInfrastructe();
        }
    }

    public void SwitchStructureOpening()
    {
        if(InfrastructureManager.GetCurrentSelectedStructure != null)
        {
            SetStructureOpen(!InfrastructureManager.GetCurrentSelectedStructure.IsOpen);
        }
    }

    public void SetStructureOpen(bool isOpen)
    {
        if(InfrastructureManager.GetCurrentSelectedStructure != null)
        {
            if (isOpen)
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
