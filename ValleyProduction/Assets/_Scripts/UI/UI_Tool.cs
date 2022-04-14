using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UI_Tool : MonoBehaviour
{
    [SerializeField] private UnityEvent OnSelectPlaceTool;
    [SerializeField] private UnityEvent OnSelectMoveTool;
    [SerializeField] private UnityEvent OnSelectDeleteTool;
    [SerializeField] private UnityEvent OnDeselectTool;

    [SerializeField] private List<UI_InfrastructureButton> structureButtons;

    public void UnlockStructure(InfrastructurePreview toUnlock) //CODE REVIEW : Voir pour utiliser des Scriptable plutot que le Preview directement
    {
        for(int i = 0; i < structureButtons.Count; i++)
        {
            if(structureButtons[i].Structure == toUnlock)
            {
                structureButtons[i].Enable();
            }
        }
    }

    public void OnToolCreatePath(int i)
    {
        ConstructionManager.SelectInfrastructureType(null);

        if (i != 0 && InfrastructureManager.GetCurrentTool != (ToolType)i)
        {
            InfrastructureManager.SetToolSelected((ToolType)i);
        }
        else
        {
            InfrastructureManager.SetToolSelected(ToolType.None);
        }

        switch (InfrastructureManager.GetCurrentTool)
        {
            case ToolType.Place:
                OnBoardingManager.OnClickBuild?.Invoke(true);
                OnSelectPlaceTool?.Invoke();
                break;
            case ToolType.Move:
                OnBoardingManager.OnClickModify?.Invoke(true);
                OnSelectMoveTool?.Invoke();
                break;
            case ToolType.Delete:
                OnSelectDeleteTool?.Invoke();
                break;
            case ToolType.None:
                OnDeselectTool?.Invoke();
                UnselectTool();
                break;
        }
    }

    public void UnselectTool()
    {
        InfrastructureManager.SetToolSelected(ToolType.None);

        if (PathManager.IsOnCreatePath)
        {
            PathManager.CreatePathData();
        }

        ConstructionManager.SelectInfrastructureType(null);
    }
}
