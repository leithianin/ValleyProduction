using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InfrastructureManager : VLY_Singleton<InfrastructureManager>
{
    [SerializeField] private List<InfrastructureData> startingStructure;

    [SerializeField] private InfrastructurePreviewHandler previewHandler;

    [SerializeField] private Infrastructure currentSelectedStructure;

    public static InfrastructurePreview GetCurrentPreview => instance.previewHandler.GetPreview;
    public static Infrastructure GetCurrentSelectedStructure => instance.currentSelectedStructure;

    public static ToolType GetCurrentTool => instance.toolSelected;

    private static LayerMask layerIgnoreRaycast = 2;
    private static LayerMask layerInfrastructure = 0;

    [SerializeField] private GameObject movedObject = null;

    [Header("Tool management")]
    public ToolType toolSelected = ToolType.None;

    private List<ToolType> disableTools = new List<ToolType>();

    [SerializeField] private UnityEvent OnSelectConstructionTool;
    [SerializeField] private UnityEvent OnUnselectConstructionTool;
    [SerializeField] private UnityEvent OnSelectModifyTool;
    [SerializeField] private UnityEvent OnUnselectModifyTool;
    [SerializeField] private UnityEvent OnSelectDeleteTool;
    [SerializeField] private UnityEvent OnUnselectDeleteTool;
    [SerializeField] private UnityEvent OnSelectNoTool;

    public static GameObject GetMovedObject => instance.movedObject;

    [Header("Others")]
    public Vector3 saveScreenPos;

    #region Actions
    public static Action<Infrastructure> OnPlaceInfrastructure;
    public static Action<Infrastructure> OnSelectInfrastructure;
    public static Action<Infrastructure> OnStartMoveInfrastructure;
    public static Action<Infrastructure> OnEndMoveInfrastructure;
    public static Action<Infrastructure> OnDestroyInfrastructure;
    #endregion
    
    private void Start()
    {
        foreach(InfrastructureData infra in startingStructure)
        {
            UIManager.UnlockStructure(infra);
        }
    }

    private void Update()
    {
        if(movedObject != null && currentSelectedStructure != null && currentSelectedStructure.StructureType == InfrastructureType.Path)
        {
            movedObject.transform.position = PlayerInputManager.GetMousePosition;
        }
    }

    public static void EnableOrDisableTool(ToolType tooltype, bool isEnable)
    {
        if (isEnable && instance.disableTools.Contains(tooltype))
        {
            instance.disableTools.Remove(tooltype);
        }
        else if (!isEnable && !instance.disableTools.Contains(tooltype))
        {
            instance.disableTools.Add(tooltype);

            if(instance.toolSelected == tooltype)
            {
                SetToolSelected(ToolType.None);
            }
        }
    }

    public void UnselectTool()
    {
        SetToolSelected(ToolType.None);
    }

    public static void SetToolSelected(ToolType toolType)
    {
        //Gestion de désélection de l'outil précédent
        if (toolType != instance.toolSelected)
        {
            switch (instance.toolSelected)
            {
                case ToolType.Place:
                    instance.EndRotation();
                    UnselectInfrastructure();
                    instance.OnUnselectConstructionTool?.Invoke();
                    break;
                case ToolType.Move:
                    instance.EndRotation();
                    if (instance.movedObject != null)
                    {
                        CancelMoveStructure();
                    }
                    instance.OnUnselectModifyTool?.Invoke();
                    break;
                case ToolType.Delete:
                    instance.OnUnselectDeleteTool?.Invoke();
                    break;
            }


            if (!instance.disableTools.Contains(toolType))
            {
                instance.toolSelected = toolType;

                switch (instance.toolSelected)
                {
                    case ToolType.Place:
                        instance.OnSelectConstructionTool?.Invoke();
                        break;
                    case ToolType.Move:
                        instance.OnSelectModifyTool?.Invoke();
                        break;
                    case ToolType.Delete:
                        instance.OnSelectDeleteTool?.Invoke();
                        break;
                    case ToolType.None:
                        instance.OnSelectNoTool?.Invoke();
                        break;
                }
            }
        }
    }

    public static void ChooseInfrastructure(InfrastructurePreview newPreview)
    {
        if(newPreview != GetCurrentPreview)
        {
            instance.previewHandler.SetInfrastructurePreview(newPreview);
        }
    }

    public static void PlaceInfrastructure(Vector3 positionToPlace)
    {
        instance.PlaceInfrastructure(GetCurrentPreview, positionToPlace);
    }

    /// <summary>
    /// Rotate the infrastructure
    /// </summary>
    /// <param name="toPlace"></param>
    /// <param name="positionToPlace"></param>
    public void RotateInfrastructure(Vector3 positionToPlace)
    {
        if (GetCurrentPreview != null && GetCurrentPreview.AskToPlace(positionToPlace) && !previewHandler.snaping)
        {
            StartRotation();
        }
    }

    /// <summary>
    /// Set all the variable needed for start the rotation
    /// </summary>
    public void StartRotation()
    {
        CursorControl.SetSaveMousePosition();

        previewHandler.isRotating = true;
        Cursor.visible = false;
    }

    /// <summary>
    /// Set all the variable used to default at the end of the rotation
    /// </summary>
    public void EndRotation()
    {
        previewHandler.isRotating = false;
        previewHandler.transform.rotation = Quaternion.identity;
        Cursor.visible = true;
    }

    int objectIndex = 0;

    private void PlaceInfrastructure(InfrastructurePreview toPlace, Infrastructure selectedStructure)
    {
        if (toPlace != null)
        {
            if (ConstructionManager.GetSelectedStructureType == selectedStructure.StructureType && toPlace.AskToPlace(selectedStructure.transform.position))
            {
                currentSelectedStructure = selectedStructure;
                selectedStructure.PlaceObject();
            }
        }
    }

    private bool PlaceInfrastructure(InfrastructurePreview toPlace, Vector3 positionToPlace)
    {
        bool toReturn = false;
        if (toPlace.AskToPlace(positionToPlace) && !previewHandler.snaping)
        {
            Infrastructure placedInfrastructure = Instantiate(toPlace.RealInfrastructure, previewHandler.transform.position, previewHandler.transform.rotation);

            placedInfrastructure.PlaceObject(positionToPlace);
            objectIndex++;
            placedInfrastructure.gameObject.name += " : " + objectIndex;

            OnPlaceInfrastructure?.Invoke(placedInfrastructure);
            toReturn = true;
        }

        EndRotation();

        return toReturn;
    }

    public static void DeleteInfrastructure(Infrastructure toDelete)
    {
        if (toDelete == instance.currentSelectedStructure)
        {
            UnselectInfrastructure();
        }

        DesnapInfrastructure(toDelete);

        OnDestroyInfrastructure?.Invoke(toDelete);

        toDelete.RemoveObject();
    }

    /// <summary>
    /// D�place l'infrastructure lors du maintient du clic.
    /// </summary>
    /// <param name="toMove"></param>
    public static void MoveInfrastructure(Infrastructure toMove)
    {
        instance.currentSelectedStructure = toMove;

        if (instance.movedObject == null)
        {
            instance.currentSelectedStructure.StartMoveObject();
        }
        instance.movedObject = toMove.gameObject;
        instance.movedObject.layer = layerIgnoreRaycast;

        instance.currentSelectedStructure.MoveObject();
        /*if (instance.currentSelectedStructure.StructureType != InfrastructureType.Path)
        {
            instance.previewHandler.SetInfrastructurePreview(toMove.Data.Preview);
            instance.previewHandler.transform.rotation = toMove.transform.rotation;
        }
        else
        {
            //Pathpoint
            OnStartMoveInfrastructure?.Invoke(instance.currentSelectedStructure);
        }*/

        instance.previewHandler.SetInfrastructurePreview(toMove.Data.Preview);

        instance.previewHandler.transform.rotation = toMove.transform.rotation;

        if (instance.currentSelectedStructure.StructureType == InfrastructureType.Path)
        {
            OnStartMoveInfrastructure?.Invoke(instance.currentSelectedStructure);
        }
    }

    public static void CancelMoveStructure()
    {
        //CODE REVIEW : PLUS UTILISE ?
        instance.EndRotation();
        GameObject saveObject = instance.movedObject;

        TimerManager.CreateRealTimer(0.5f, () => ReplaceInfrastructureChangeLyer(saveObject));
        instance.movedObject = null;

        instance.previewHandler.SetInfrastructurePreview(null);

        OnEndMoveInfrastructure?.Invoke(instance.currentSelectedStructure);

        instance.currentSelectedStructure.CancelMoveObject();
    }

    public static void OnHoldRightClic(InfrastructureType tool, Infrastructure toHoldRightClic)
    {
        instance.currentSelectedStructure = toHoldRightClic;
        instance.currentSelectedStructure.HoldRightClic();
    }

    /// <summary>
    /// Place l'infrastructure d�plac� lorsqu'on l�che le maintient.
    /// </summary>
    public static void ReplaceInfrastructure(Vector3 position)
    {
        Debug.Log("Replace Structure");
        //Pathpoint
        /*if (instance.currentSelectedStructure.StructureType == InfrastructureType.Path)
        {
            instance.previewHandler.SetInfrastructurePreview(GetCurrentSelectedStructure.Data.Preview);
        }*/

        if (GetCurrentPreview.CanPlaceObject(position))
        {
            Debug.Log(GetCurrentPreview.CanPlaceObject(position));

            instance.ChangeInfrastructurePosition(GetCurrentSelectedStructure, position);

            instance.movedObject.layer = default;
            instance.movedObject = null;

            if (instance.currentSelectedStructure.StructureType == InfrastructureType.Path)
            {
                OnPlaceInfrastructure?.Invoke(GetCurrentSelectedStructure);
            }

            instance.previewHandler.SetInfrastructurePreview(null);

            OnEndMoveInfrastructure?.Invoke(instance.currentSelectedStructure);

            instance.currentSelectedStructure.ReplaceObject();
        }
        else
        {
            CancelMoveStructure();
        }

    }

    private void ChangeInfrastructurePosition(Infrastructure toChange, Vector3 position)
    {
        if (GetCurrentPreview.AskToPlace(position) && !previewHandler.snaping)
        {
            toChange.transform.rotation = previewHandler.transform.rotation;

            toChange.transform.position = position;

            toChange.ReplaceObject();
        }
        EndRotation();
    }

    public static void ReplaceInfrastructureChangeLyer(GameObject saveObject)
    {
        saveObject.layer = layerInfrastructure;
    }

    public static void InteractWithStructure(ToolType tool, Infrastructure interactedStructure)
    {
        if(instance.currentSelectedStructure != interactedStructure)
        {
            UnselectInfrastructure();
        }

        if (interactedStructure.CanBeUsedByTool(tool))
        {
            switch (tool)
            {
                //Just select l'infrastructure (Info)
                case ToolType.None:
                    instance.SelectInfrastructure(interactedStructure);
                    break;
                case ToolType.Place:
                    instance.PlaceInfrastructure(GetCurrentPreview, interactedStructure);
                    break;
                case ToolType.Move:
                    MoveInfrastructure(interactedStructure);
                    break;
                case ToolType.Delete:
                    DeleteInfrastructure(interactedStructure);
                    break;
            }
        }
    }

    /// <summary>
    /// S�lectionne l'Infrastructure.
    /// </summary>
    /// <param name="selectedStructure">L'Infrastructure � s�lectionner.</param>
    private void SelectInfrastructure(Infrastructure selectedStructure)
    {
        currentSelectedStructure = selectedStructure;
        selectedStructure.SelectObject();

        OnSelectInfrastructure?.Invoke(selectedStructure);
    }

    /// <summary>
    /// D�s�lectionne l'Infrastructure.
    /// </summary>
    public static void UnselectInfrastructure()
    {
        instance.EndRotation();
        if (instance.currentSelectedStructure != null)
        {
            Debug.Log(instance.currentSelectedStructure.gameObject);
            instance.currentSelectedStructure.UnselectObject();
        }
        instance.currentSelectedStructure = null;
    }

    public static void SnapInfrastructure(Infrastructure infrastructure)
    {
        if (!instance.previewHandler.isRotating)
        {
            instance.previewHandler.SetSnaping(true);
            instance.previewHandler.transform.position = infrastructure.transform.position;
        }
    }

    public static void DesnapInfrastructure(Infrastructure infrastructure)
    {
        instance.previewHandler.SetSnaping(false);
    }

    public static void ShowPreviewDisplay()
    {
        instance.previewHandler.ShowDisplay();
    }

    public static void HidePreviewDisplay()
    {
        instance.previewHandler.HideDisplay();
    }

    private void OnDestroy()
    {
        OnPlaceInfrastructure = null;
        OnSelectInfrastructure = null; 
        OnStartMoveInfrastructure = null;
        OnEndMoveInfrastructure = null;
        OnDestroyInfrastructure = null;
    }
}
