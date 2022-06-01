using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_InfrastructureButton : MonoBehaviour
{
    [SerializeField] private InfrastructureData structure;
    [SerializeField] private Button button;

    [SerializeField] private UnityEvent<InfrastructureData> OnSelectStructure;

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

        //OnSelectStructure?.Invoke(structure);

        ConstructionManager.SelectInfrastructureType(structure);
    }
}
