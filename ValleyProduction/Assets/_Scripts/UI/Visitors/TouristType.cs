using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TouristType : MonoBehaviour
{
    public TextMeshProUGUI name;

    [Header("Pollution")]
    public Image pollution;
    public TextMeshProUGUI pollutionText;
    [Header("Noise")]
    public Image noise;
    public TextMeshProUGUI noiseText;
    [Header("Stamina")]
    public Image stamina;
    public TextMeshProUGUI staminaText;
    public TextMeshProUGUI goal;
    public TextMeshProUGUI note;
}
