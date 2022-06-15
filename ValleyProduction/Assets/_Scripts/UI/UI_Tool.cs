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
    [SerializeField] private UnityEvent OnSelectStructure;
    [SerializeField] private UnityEvent OnUnselectStructure;

    [SerializeField] private List<UI_InfrastructureButton> structureButtons;

    [SerializeField] private InfrastructureData currentStructure;

    public void UnlockStructure(InfrastructureData toUnlock) //CODE REVIEW : Voir pour utiliser des Scriptable plutot que le Preview directement
    {
        for(int i = 0; i < structureButtons.Count; i++)
        {
            if (structureButtons[i].Structure == toUnlock)
            {
                structureButtons[i].Enable();
            }
        }
    }

    public void OnToolCreatePath(int i)
    {
        if (!PlayerInputManager.BlockMouse)
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

            Debug.Log(InfrastructureManager.GetCurrentTool);

            switch (InfrastructureManager.GetCurrentTool)
            {
                case ToolType.Place:
                    OnSelectPlaceTool?.Invoke();
                    break;
                case ToolType.Move:
                    OnSelectMoveTool?.Invoke();
                    break;
                case ToolType.Delete:
                    OnSelectDeleteTool?.Invoke();
                    break;
                case ToolType.None:
                    UnselectTool();
                    break;
            }
        }
    }

    public void UnselectTool()
    {
        OnDeselectTool?.Invoke();

        Debug.Log("OnDeselectTool");

        if (PathManager.IsOnCreatePath)
        {
            PathManager.CreatePathData();
        }
    }

    public void SelectStructure(InfrastructureData structure)
    {
        if (currentStructure != structure)
        {
            if (structure != null)
            {
                OnSelectStructure?.Invoke();

                currentStructure = structure;
            }
            else
            {
                UnselectStructure(currentStructure);
            }
        }
        else
        {
            UnselectStructure(currentStructure);
        }
    }

    public void UnselectStructure(InfrastructureData structure)
    {
        foreach(UI_InfrastructureButton button in structureButtons)
        {
            if(button.Structure == structure)
            {
                button.UnselectStructure();
            }
        }

        OnUnselectStructure?.Invoke();

        currentStructure = null;
    }
}
