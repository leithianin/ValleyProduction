using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfrastructureManager : VLY_Singleton<InfrastructureManager>
{
    [SerializeField] private InfrastructurePreviewHandler previewHandler;
    
    private InfrastructurePreview currentPreview;

    public static InfrastructurePreview GetCurrentPreview => instance.currentPreview;


    public static void ChooseInfrastructure(InfrastructurePreview newPreview)
    {
        if(newPreview != instance.currentPreview)
        {
            instance.previewHandler.SetInfrastructurePreview(newPreview);
        }
        instance.currentPreview = newPreview;
    }
    
    public static void PlaceInfrastructure(Vector3 positionToPlace)
    {
        instance.PlaceInfrastructure(GetCurrentPreview, positionToPlace);
    }

    private bool PlaceInfrastructure(InfrastructurePreview toPlace, Vector3 positionToPlace)
    {
        if(toPlace.AskToPlace(positionToPlace))
        {
            Infrastructure placedInfrastructure = Instantiate(toPlace.RealInfrastructure, positionToPlace, Quaternion.identity);

            placedInfrastructure.PlaceObject(positionToPlace);
            return true;
        }
        return false;
    }

    public static void PlaceOnExistingInfrastructure(Infrastructure existingConstruction)
    {

    }

    public static void ModifyInfrastructure(Infrastructure toModify)
    {

    }

    public static void DeleteInfrastructure(Infrastructure toDelete)
    {
        toDelete.RemoveObject();
    }
}
