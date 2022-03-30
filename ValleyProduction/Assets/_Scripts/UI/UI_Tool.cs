using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UI_Tool : MonoBehaviour
{
    [SerializeField] private UnityEvent OnSelectPlaceTool;
    [SerializeField] private UnityEvent OnSelectMoveTool;
    [SerializeField] private UnityEvent OnSelectDeleteTool;

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


    #region Build
    public void SelectStructure(InfrastructurePreview structure)
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
    #endregion
}
