using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TouristType : MonoBehaviour
{
    public TextMeshProUGUI name;                        //Visitor's name

    [Header("Noise")]
    public Image noiseBackground;                       //Noise Background image
    public Image noiseLogo;                             //Noise Logo image
    public TextMeshProUGUI noiseText;

    [Header("Pollution")]
    public Image pollutionBackground;                   //Pollution Background image
    public Image pollutionLogo;                         //Pollution Logo image
    public TextMeshProUGUI pollutionText;

    [Header("Stamina")]
    public Image stamina;
    public TextMeshProUGUI staminaText;
    public TextMeshProUGUI goal;
    public TextMeshProUGUI note;

    [Header("Tab")]
    private GameObject currentTab;

    public void ChangeCurrentTab(GameObject go)
    {
        currentTab.SetActive(false);
        go.SetActive(true);
        currentTab = go;
    }
}
