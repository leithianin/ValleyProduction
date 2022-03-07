using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfrastructureManager : VLY_Singleton<InfrastructureManager>
{
    [SerializeField] private InfrastructurePreviewHandler previewHandler;

    private Infrastructure currentSelectedStructure;

    public static InfrastructurePreview GetCurrentPreview => instance.previewHandler.GetPreview;
    public static Infrastructure GetCurrentSelectedStructure => instance.currentSelectedStructure;

    public static ToolType GetCurrentTool => instance.toolSelected;

    private static LayerMask layerIgnoreRaycast = 2;
    private static LayerMask layerInfrastructure = 0;

    private GameObject movedObject = null;
    public ToolType toolSelected = ToolType.None;

    public static GameObject GetMovedObject => instance.movedObject;


    private void Update()
    {
        //Move Infrastructure when MoveInfrastructure()
        if(movedObject != null)
        {
            movedObject.transform.position = PlayerInputManager.GetMousePosition;
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

    private void PlaceInfrastructure(Infrastructure selectedStructure)
    {
        if (ConstructionManager.GetSelectedStructureType == selectedStructure.StructureType)
        {
            currentSelectedStructure = selectedStructure;
            selectedStructure.PlaceObject();
        }
    }

    private bool PlaceInfrastructure(InfrastructurePreview toPlace, Vector3 positionToPlace)
    {
        if (toPlace.AskToPlace(positionToPlace) && !previewHandler.snaping)
        {
            Infrastructure placedInfrastructure = Instantiate(toPlace.RealInfrastructure, positionToPlace, Quaternion.identity);

            placedInfrastructure.PlaceObject(positionToPlace);
            return true;
        }
        return false;
    }

    public static void DeleteInfrastructure(Infrastructure toDelete)
    {
        if (toDelete == instance.currentSelectedStructure)
        {
            UnselectInfrastructure();
        }
        toDelete.RemoveObject();
    }

    /// <summary>
    /// Déplace l'infrastructure lors du maintient du clic.
    /// </summary>
    /// <param name="toMove"></param>
    public static void MoveInfrastructure(Infrastructure toMove)
    {
        instance.currentSelectedStructure = toMove;
        instance.movedObject = toMove.gameObject;
        instance.movedObject.layer = layerIgnoreRaycast;
        instance.currentSelectedStructure.MoveObject();
    }

    public static void OnHoldRightClic(InfrastructureType tool, Infrastructure toHoldRightClic)
    {
        instance.currentSelectedStructure = toHoldRightClic;
        instance.currentSelectedStructure.HoldRightClic();
    }

    /// <summary>
    /// Place l'infrastructure déplacé lorsqu'on lâche le maintient.
    /// </summary>
    public static void ReplaceInfrastructure(Vector3 position)
    {
        instance.movedObject.layer = layerInfrastructure;
        instance.movedObject = null;
        instance.currentSelectedStructure.ReplaceObject();
    }

    public static void InteractWithStructure(ToolType tool, Infrastructure interactedStructure)
    {
        switch (tool)
        {
            //Just select l'infrastructure (Info)
            case ToolType.None:
                instance.SelectInfrastructure(interactedStructure);
                //instance.SelectInfrastructure(interactedStructure);
                break;      
            case ToolType.Place:
                instance.PlaceInfrastructure(interactedStructure);
                //Usable by balise, je suis entrain de placer, je clique sur une infrastructure.
                break;
            case ToolType.Move:
                Debug.Log("dd");
                MoveInfrastructure(interactedStructure);
                break;
            case ToolType.Delete:
                DeleteInfrastructure(interactedStructure);
                break;
        }
    }

    /// <summary>
    /// Sélectionne l'Infrastructure.
    /// </summary>
    /// <param name="selectedStructure">L'Infrastructure à sélectionner.</param>
    private void SelectInfrastructure(Infrastructure selectedStructure)
    {
        //if (selectedStructure != currentSelectedStructure)
        {
            currentSelectedStructure = selectedStructure;
            selectedStructure.SelectObject();
        }
    }

    /// <summary>
    /// Désélectionne l'Infrastructure.
    /// </summary>
    public static void UnselectInfrastructure()
    {
        if(instance.currentSelectedStructure != null)
        {
            instance.currentSelectedStructure.UnselectObject();
        }
        instance.currentSelectedStructure = null;
    }

    public static void SetCurrentSelectedStructureToNull()
    {
        instance.currentSelectedStructure = null;
    }

    public static void SnapInfrastructure(Infrastructure infrastructure)
    {
        instance.previewHandler.snaping = true;
        instance.previewHandler.transform.position = infrastructure.transform.position;
    }

    public static void DesnapInfrastructure(Infrastructure infrastructure)
    {
        instance.previewHandler.snaping = false;
    }
}
