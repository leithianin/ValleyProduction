using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class UI_InfrastructureButton : MonoBehaviour
{
    [SerializeField] private InfrastructureData structure;
    [SerializeField] private Button button;

    [Header("Button")]
    [SerializeField] private Image buttonIcon;

    [Header("Tooltip")]
    [SerializeField] private TextMeshProUGUI nameHolder;
    [SerializeField] private TextMeshProUGUI descriptionHolder;
    [SerializeField] private Image exempleImage;
    [SerializeField] private UI_DataHandler noiseScore;
    [SerializeField] private UI_DataHandler pollutionScore;
    [SerializeField] private UI_DataHandler capacityScore;
    [SerializeField] private TextMeshProUGUI priceHolder;

    [Header("Events")]
    [SerializeField] private UnityEvent onSelectStructure;
    [SerializeField] private UnityEvent onUnselectStructure;

    private bool isSelected;

    public InfrastructureData Structure => structure;

    public void Enable()
    {
        button.interactable = true;
    }

    public void SelectStructure()
    {
        switch (structure.Structure.StructureType)
        {
            case InfrastructureType.Path:
                if (PathManager.IsOnCreatePath)
                {
                    PathManager.CreatePathData();
                }
                break;
        }

        if (!isSelected)
        {
            onSelectStructure?.Invoke();
        }

        isSelected = true;

        ConstructionManager.SelectInfrastructureType(structure);
    }

    public void UnselectStructure()
    {
        isSelected = false;

        onUnselectStructure?.Invoke();
    }

    [ContextMenu("Set structure")]
    public void SetSelfStructure()
    {
        SetStructure(structure);
    }

    private void SetStructure(InfrastructureData nStructure)
    {
        structure = nStructure;

        nameHolder.text = structure.Name;
        descriptionHolder.text = structure.Description;

        buttonIcon.sprite = structure.ButtonIcon;
        exempleImage.sprite = structure.ExempleImage;

        ECO_AGT_Informations structureScores = structure.Structure.GetComponent<ECO_AGT_Informations>();
        {
            noiseScore.SetScore(structureScores.GetNoiseScore());

            pollutionScore.SetScore(structureScores.GetPolluterScore());
        }

        priceHolder.text = structure.Cost().ToString();

        if (structure.Structure.interestPoint != null)
        {
            capacityScore.SetScore(structure.Structure.interestPoint.GetInteractionMaxVisitors());
        }
        else
        {
            capacityScore.SetScore(0);
        }

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}
