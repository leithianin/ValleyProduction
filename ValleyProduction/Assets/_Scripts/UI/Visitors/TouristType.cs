using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TouristType : MonoBehaviour
{
    public TextMeshProUGUI name;                        //Visitor's name

    [Header("Noise")]
    public List<TextMeshProUGUI> noiseText;
    public List<Image> noiseListBackground;
    public List<Image> noiseListImage;

    [Header("Pollution")]
    public List<TextMeshProUGUI> pollutionText;
    public List<Image> pollutionListBackground;
    public List<Image> pollutionListImage;

    [Header("Stamina")]
    public Image stamina;
    public TextMeshProUGUI staminaText;
    public TextMeshProUGUI goal;
    public TextMeshProUGUI note;

    [Header("Tab")]
    private GameObject currentTab;

    public void ChangeCurrentTab(GameObject go)
    {
        if (currentTab != null)
        {
            currentTab.SetActive(false);
        }
        go.SetActive(true);
        currentTab = go;
    }
}
