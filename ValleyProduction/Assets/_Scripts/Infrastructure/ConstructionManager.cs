using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConstructionManager : VLY_Singleton<ConstructionManager>
{
    private InfrastructureType selectedStructureType = InfrastructureType.None;
    private InfrastructureData selectedStructureData = null;

    public UnityEvent OnSelectPathTool;
    public UnityEvent OnUnselectPathTool;
    public UnityEvent OnUnselectOneMore;

    public static bool HasSelectedStructureType => instance.selectedStructureType != InfrastructureType.None;// && instance.selectedStructureType != InfrastructureType.DeleteStructure;

    public static InfrastructureType GetSelectedStructureType => instance.selectedStructureType;

    /// <summary>
    /// Prend en compte l'Input pour placer une infrastructure.
    /// </summary>
    /// <param name="posePosition">La position du clic.</param>
    public static void PlaceInfrastructure(Vector3 posePosition)
    {
        if (InfrastructureManager.GetCurrentTool == ToolType.Place && HasSelectedStructureType)
        {
            InfrastructureManager.PlaceInfrastructure(posePosition);
        }

        if(InfrastructureManager.GetCurrentTool == ToolType.Move && InfrastructureManager.GetMovedObject != null)
        {
            InfrastructureManager.ReplaceInfrastructure(posePosition);
        }
    }

    public static void CancelRotation()
    {
        //Sert � rien de passer dedans si on est en path
        InfrastructureManager.instance.EndRotation();
    }

    /// <summary>
    /// Prend en compte l'Input quand on clic sur une Infrastructure.
    /// </summary>
    /// <param name="touchedObject">L'objet touch�.</param>
    public static void InteractWithStructure(GameObject touchedObject)
    {
        Infrastructure infraComponent = touchedObject.GetComponent<Infrastructure>();
        if (infraComponent != null)
        {
            InfrastructureManager.InteractWithStructure(InfrastructureManager.GetCurrentTool, infraComponent);
        }
        else
        {
            UIManager.HideShownGameObject();
        }
    }

    public static void OnHoldRightClic(GameObject touchedObject)
    {
        Infrastructure infraComponent = touchedObject.GetComponent<Infrastructure>();
        if (infraComponent != null)
        {
            InfrastructureManager.OnHoldRightClic(instance.selectedStructureType, infraComponent);
        }
    }

    /// <summary>
    /// Prend en compte l'input quand on maintient le clic
    /// </summary>
    /// <param name="touchedObject"></param>
    public static void MoveStructure(GameObject touchedObject)
    {
        //InfrastructureManager.MoveInfrastructure(touchedObject.GetComponent<Infrastructure>());
    }

    /// <summary>
    /// Prend en compte l'input quand on l�che le maintien du clic
    /// </summary>
    public static void ReplaceStructure()
    {
        //InfrastructureManager.ReplaceInfrastructure();
    }

    public static void DestroyStructure(GameObject touchedObject) //CODE REVIEW : Voir pour le mettre dans le PathCreationManager
    {
        if (PathManager.previousPathpoint != null && PathManager.previousPathpoint.gameObject == touchedObject)
        {
            InfrastructureManager.InteractWithStructure(ToolType.Delete, PathManager.previousPathpoint);
        }
        /*if (PathManager.GetCurrentPathData != null)
        {
            Infrastructure infraComponent = touchedObject.GetComponent<Infrastructure>();
            if (infraComponent != null)
            {
                //InfrastructureManager.InteractWithStructure(InfrastructureType.DeleteStructure, infraComponent);
                InfrastructureManager.InteractWithStructure(ToolType.Delete, infraComponent);
            }
            else
            {
                if (instance.selectedStructureType == InfrastructureType.Path)
                {
                    if (PathManager.previousPathpoint != null)
                    {
                        //InfrastructureManager.InteractWithStructure(InfrastructureType.DeleteStructure, PathManager.previousPathpoint);
                        InfrastructureManager.InteractWithStructure(ToolType.Delete, PathManager.previousPathpoint);
                    }
                }
            }
        }
        else
        {
            if (PathManager.previousPathpoint == touchedObject)
            {
                //InfrastructureManager.InteractWithStructure(InfrastructureType.DeleteStructure, PathManager.previousPathpoint);
                InfrastructureManager.InteractWithStructure(ToolType.Delete, PathManager.previousPathpoint);
            }
        }*/
    }

    //C'�tait pour placer sur une infrastructure (Cr�er � partir d'une infrastructure)
    public static void PlaceOnStructure(GameObject touchedObject)
    {
        PathManager.CreatePathData();
        PathManager.PlacePoint(touchedObject.GetComponent<IST_PathPoint>());
    }

    /// <summary>
    /// Input de d�s�lection d'Infrastructure.
    /// </summary>
    public static void UnselectStructure()
    {
        InfrastructureManager.UnselectInfrastructure();
    }

    /// <summary>
    /// Input de s�lection de l'outil.
    /// </summary>
    /// <param name="newStructureType">L'outil s�lectionn�.</param>
    public static void SelectInfrastructureType(InfrastructureData newStructureType)
    {
        instance.OnSelectInfrastructureType(newStructureType);
    }

    /// <summary>
    /// Input pour d�s�lectionner l'outil.
    /// </summary>
    public static void UnselectInfrastructureType()
    {
        if(!HasSelectedStructureType)
        {
            if (UIManager.IsOnMenuBool()) { UIManager.HideMenuOption()          ; }
            else                             { instance.OnUnselectOneMore?.Invoke(); }
        }

        InfrastructureManager.SetCurrentSelectedStructureToNull();                                                          //Reset CurrentSelectedStructure
        instance.OnSelectInfrastructureType(null);
    }

    /// <summary>
    /// G�re le changement d'outil.
    /// </summary>
    /// <param name="newStructureType">L'outil s�lectionn�.</param>
    private void OnSelectInfrastructureType(InfrastructureData newStructureType)
    {
        InfrastructureData lastStructureData = selectedStructureData;

        OnUnselectInfrastructureType();
        if (newStructureType != null && lastStructureData != newStructureType)
        {
            selectedStructureType = newStructureType.Structure.StructureType;
            selectedStructureData = newStructureType;

            InfrastructureManager.ChooseInfrastructure(newStructureType.Preview);

            switch (selectedStructureType)
            {
                case InfrastructureType.Path:
                    VLY_ContextManager.ChangeContext(1);
                    OnSelectPathTool?.Invoke();
                    break;
            }
        }
    }

    /// <summary>
    /// G�re la d�s�lection de l'outil.
    /// </summary>
    private void OnUnselectInfrastructureType()
    {
        InfrastructureManager.ChooseInfrastructure(null);

        switch(selectedStructureType)
        {
            case InfrastructureType.Path:
                PathManager.CreatePathData();
                VLY_ContextManager.ChangeContext(0);
                OnUnselectPathTool?.Invoke();
                break;
        }

        selectedStructureType = InfrastructureType.None;
        selectedStructureData = null;
    }
}
