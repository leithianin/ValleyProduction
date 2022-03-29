using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Tool : MonoBehaviour
{
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
                break;
            case ToolType.Move:
                OnBoardingManager.OnClickModify?.Invoke(true);
                break;
            case ToolType.Delete:
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
