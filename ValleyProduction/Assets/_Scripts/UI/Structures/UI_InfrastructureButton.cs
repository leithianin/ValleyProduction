using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InfrastructureButton : MonoBehaviour
{
    [SerializeField] private InfrastructurePreview structure;
    [SerializeField] private Button button;

    public InfrastructurePreview Structure => structure;
    
    public void Enable()
    {
        button.interactable = true;
    }

    public void SelectStructure()
    {
        switch (structure.RealInfrastructure.StructureType)
        {
            case InfrastructureType.Path:
                if (PathManager.IsOnCreatePath)
                {
                    PathManager.CreatePathData();
                }
                break;
        }

        ConstructionManager.SelectInfrastructureType(structure);
    }
}
