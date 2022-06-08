using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UI_StructureButton : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private Image buttonIcon;

    [Header("Tooltip")]
    [SerializeField] private TextMeshProUGUI nameHolder;
    [SerializeField] private TextMeshProUGUI descriptionHolder;
    [SerializeField] private Image exempleImage;
    [SerializeField] private TextMeshProUGUI noiseScore;
    [SerializeField] private TextMeshProUGUI pollutionScore;
    [SerializeField] private TextMeshProUGUI capacityScore;
    [SerializeField] private TextMeshProUGUI priceHolder;
}
