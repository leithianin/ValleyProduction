using UnityEngine;
using UnityEngine.Events;

public class ConstructionManager : VLY_Singleton<ConstructionManager>
{
    private InfrastructureType selectedStructureType = InfrastructureType.None;
    private InfrastructureData selectedStructureData = null;

    public UnityEvent OnSelectPathTool;
    public UnityEvent OnUnselectPathTool;

    public UnityEvent OnUnselectOneMore;
    [SerializeField] private UnityEvent<InfrastructureData> OnSelectStructure;
    [SerializeField] private UnityEvent<InfrastructureData> OnUnselectStructure;

    public static bool HasSelectedStructureType => instance.selectedStructureType != InfrastructureType.None;// && instance.selectedStructureType != InfrastructureType.DeleteStructure;

    public static InfrastructureType GetSelectedStructureType => instance.selectedStructureType;

    /// <summary>
    /// Prend en compte l'Input pour placer une infrastructure.
    /// </summary>
    /// <param name="posePosition">La position du clic.</param>
    public static void PlaceInfrastructure(Vector3 posePosition)
    {
        if (InfrastructureManager.GetCurrentTool == ToolType.Place && HasSelectedStructureType && InfrastructureManager.GetCurrentPreview != null)
        {
            InfrastructureManager.PlaceInfrastructure(posePosition);
        }

        if (InfrastructureManager.GetCurrentTool == ToolType.Move && InfrastructureManager.GetMovedObject != null)
        {
            InfrastructureManager.ReplaceInfrastructure(posePosition);
        }
    }

    public static void CancelRotation()
    {
        //Sert à rien de passer dedans si on est en path
        InfrastructureManager.instance.EndRotation();
    }

    /// <summary>
    /// Prend en compte l'Input quand on clic sur une Infrastructure.
    /// </summary>
    /// <param name="touchedObject">L'objet touché.</param>
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
    public static void RotateStructure(Vector3 position)
    {
        InfrastructureManager.instance.RotateInfrastructure(position);
    }

    public static void DestroyStructure(GameObject touchedObject) //CODE REVIEW : Voir pour le mettre dans le PathCreationManager
    {
        if (PathManager.previousPathpoint != null && PathManager.previousPathpoint.gameObject == touchedObject)
        {
            InfrastructureManager.InteractWithStructure(ToolType.Delete, PathManager.previousPathpoint);
        }
    }

    //C'était pour placer sur une infrastructure (Créer à partir d'une infrastructure)
    public static void PlaceOnStructure(GameObject touchedObject)
    {
        PathManager.CreatePathData();
        PathManager.PlacePoint(touchedObject.GetComponent<IST_PathPoint>());
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
    public static bool SelectInfrastructureType(InfrastructureData newStructureType)
    {
        return instance.OnSelectInfrastructureType(newStructureType);
    }

    /// <summary>
    /// Input pour désélectionner l'outil.
    /// </summary>
    public static void UnselectInfrastructureType()
    {
        if (InfrastructureManager.GetCurrentPreview != null)
        {
            UnselectPreview();
        }
        else if (InfrastructureManager.GetCurrentTool != ToolType.None)
        {
            InfrastructureManager.SetToolSelected(ToolType.None);
        }
        else
        {
            instance.OnUnselectOneMore?.Invoke();
        }
    }

    public static void UnselectPreview()
    {
        switch (InfrastructureManager.GetCurrentTool)
        {
            case ToolType.Place:
                instance.IsMovingPathpoint();
                InfrastructureManager.UnselectInfrastructure();                                                          //Reset CurrentSelectedStructure
                instance.OnSelectInfrastructureType(null);
                break;
            case ToolType.Move:
                InfrastructureManager.CancelMoveStructure();
                break;
        }
    }

    /// <summary>
    /// Gère le changement d'outil.
    /// </summary>
    /// <param name="newStructureType">L'outil sélectionné.</param>
    private bool OnSelectInfrastructureType(InfrastructureData newStructureType)
    {
        bool toReturn = false;

        InfrastructureData lastStructureData = selectedStructureData;

        OnUnselectInfrastructureType();
        if (newStructureType != lastStructureData)
        {
            if (newStructureType != null)
            {
                selectedStructureType = newStructureType.Structure.StructureType;
                selectedStructureData = newStructureType;

                InfrastructureManager.ChooseInfrastructure(newStructureType.Preview);

                switch (selectedStructureType)
                {
                    case InfrastructureType.Path:
                        OnSelectPathTool?.Invoke();
                        break;
                }

                toReturn = true;
            }

            OnSelectStructure?.Invoke(selectedStructureData);
        }

        return toReturn;
    }

    /// <summary>
    /// Gère la désélection de l'outil.
    /// </summary>
    private void OnUnselectInfrastructureType()
    {
        OnUnselectStructure?.Invoke(selectedStructureData);

        InfrastructureManager.ChooseInfrastructure(null);

        switch (selectedStructureType)
        {
            case InfrastructureType.Path:
                PathManager.CreatePathData();
                OnUnselectPathTool?.Invoke();
                break;
        }

        selectedStructureData = null;
    }

    /// <summary>
    /// Check avant d'Unselect le tool si j'étais entrain de bouger un pathpoint, si Oui le mettre à sa position d'avant
    /// </summary>
    private void IsMovingPathpoint()
    {
        if (PathManager.GetppSaveMove != null)
        {
            PathCreationManager.CancelMovingAction();
            InfrastructureManager.ReplaceInfrastructure(PathManager.GetppSaveMove.transform.position);
        }
    }
}
