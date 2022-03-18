using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_InfrastructureInformation : MonoBehaviour
{
    [SerializeField] private TMP_Text nameDisplay;
    [SerializeField] private TMP_Text descriptionDisplay;
    [SerializeField] private TMP_Text pollutionScoreDisplay;
    [SerializeField] private TMP_Text noiseScoreDisplay;
    [SerializeField] private TMP_Text floraScoreDisplay;

    public void ShowStructureInformation(AU_Informations informationToShow)
    {
        nameDisplay.text = informationToShow.GetName;
        descriptionDisplay.text = informationToShow.GetDescription;

        pollutionScoreDisplay.text = informationToShow.GetPolluterScore().ToString();
        noiseScoreDisplay.text = informationToShow.GetNoiseScore().ToString();
        floraScoreDisplay.text = informationToShow.GetPlantScore().ToString();

        gameObject.SetActive(true);
    }
}
