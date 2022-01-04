using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConstructionManager : VLY_Singleton<ConstructionManager>
{
    private InfrastructureType selectedStructureType = InfrastructureType.None;

    [SerializeField] private InfrastructurePreview pathPointPreview;

    public UnityEvent OnSelectPathTool;
    public UnityEvent OnUnselectPathTool;

    public static bool HasSelectedStructureType => instance.selectedStructureType != InfrastructureType.None;

    public static void PlaceInfrastructure(Vector3 posePosition)
    {
        if(HasSelectedStructureType)
        {
            InfrastructureManager.PlaceInfrastructure(posePosition);
        }
    }

    public static void SelectInfrastructureType(InfrastructureType newStructureType)
    {
        instance.OnSelectInfrastructureType(newStructureType);
    }

    public static void UnselectInfrastructureType()
    {
        instance.OnSelectInfrastructureType(InfrastructureType.None);
    }

    private void OnSelectInfrastructureType(InfrastructureType newStructureType)
    {
        InfrastructureType lastType = selectedStructureType;
        OnUnselectInfrastructureType();
        if (newStructureType != InfrastructureType.None && lastType != newStructureType)
        {
            selectedStructureType = newStructureType;
            switch (newStructureType)
            {
                case InfrastructureType.PathTools:
                    InfrastructureManager.ChooseInfrastructure(pathPointPreview);
                    OnSelectPathTool?.Invoke();
                    break;
            }
        }
    }

    private void OnUnselectInfrastructureType()
    {
        switch (selectedStructureType)
        {
            case InfrastructureType.PathTools:
                InfrastructureManager.ChooseInfrastructure(null);
                OnUnselectPathTool?.Invoke();
                break;
        }
        selectedStructureType = InfrastructureType.None;
    }
}
