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

    public static bool HasSelectedStructureType => instance.selectedStructureType != InfrastructureType.None && instance.selectedStructureType != InfrastructureType.DeleteStructure;

    /// <summary>
    /// Prend en compte l'Input pour placer une infrastructure.
    /// </summary>
    /// <param name="posePosition">La position du clic.</param>
    public static void PlaceInfrastructure(Vector3 posePosition)
    {
        if(HasSelectedStructureType)
        {
            InfrastructureManager.PlaceInfrastructure(posePosition);
        }
    }

    /// <summary>
    /// Prend en compte l'Input quand on clic sur une Infrastructure.
    /// </summary>
    /// <param name="touchedObject">L'objet touché.</param>
    public static void InteractWithStructure(GameObject touchedObject)
    {
        Infrastructure infraComponent = touchedObject.GetComponent<Infrastructure>();
        if(infraComponent != null)
        {
            InfrastructureManager.InteractWithStructure(instance.selectedStructureType, infraComponent);
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
        InfrastructureManager.MoveInfrastructure(touchedObject.GetComponent<Infrastructure>());
    }

    /// <summary>
    /// Prend en compte l'input quand on lâche le maintien du clic
    /// </summary>
    public static void ReplaceStructure()
    {
        InfrastructureManager.ReplaceInfrastructure();
    }

    public static void DestroyStructure(GameObject touchedObject)
    {
        Infrastructure infraComponent = touchedObject.GetComponent<Infrastructure>();
        if (infraComponent != null)
        {
            InfrastructureManager.InteractWithStructure(InfrastructureType.DeleteStructure, infraComponent);
        }
        else
        {
            //Crée le chemin si déselectionne l'outil
            if(instance.selectedStructureType == InfrastructureType.PathTools)
            {
                UIManager.HideRoadsInfo();
                PathManager.CreatePathData();
            }

            UnselectInfrastructureType();
        }
    }

    //C'était pour placer sur une infrastructure (Créer à partir d'une infrastructure)
    public static void PlaceOnStructure(GameObject touchedObject)
    {
        PathManager.CreatePathData();
        PathManager.PlacePoint(touchedObject.GetComponent<IST_PathPoint>(), touchedObject.transform.position);
    }

    /// <summary>
    /// Input de désélection d'Infrastructure.
    /// </summary>
    public static void UnselectStructure()
    {
        InfrastructureManager.UnselectInfrastructure();
    }

    /// <summary>
    /// Input de sélection de l'outil.
    /// </summary>
    /// <param name="newStructureType">L'outil sélectionné.</param>
    public static void SelectInfrastructureType(InfrastructureType newStructureType)
    {
        instance.OnSelectInfrastructureType(newStructureType);
    }

    /// <summary>
    /// Input pour désélectionner l'outil.
    /// </summary>
    public static void UnselectInfrastructureType()
    {
        InfrastructureManager.SetCurrentSelectedStructureToNull();                                                          //Reset CurrentSelectedStructure
        instance.OnSelectInfrastructureType(InfrastructureType.None);
    }

    /// <summary>
    /// Gère le changement d'outil.
    /// </summary>
    /// <param name="newStructureType">L'outil sélectionné.</param>
    private void OnSelectInfrastructureType(InfrastructureType newStructureType)
    {
        InfrastructureType lastType = selectedStructureType;
        OnUnselectInfrastructureType();
        if (newStructureType != InfrastructureType.None && lastType != newStructureType)
        {
            selectedStructureType = newStructureType;

            if (newStructureType != InfrastructureType.DeleteStructure)
            {
                switch (newStructureType)
                {
                    case InfrastructureType.PathTools:
                        InfrastructureManager.ChooseInfrastructure(pathPointPreview);
                        OnSelectPathTool?.Invoke();
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Gère la désélection de l'outil.
    /// </summary>
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
