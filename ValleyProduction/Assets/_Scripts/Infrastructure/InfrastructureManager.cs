using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfrastructureManager : VLY_Singleton<InfrastructureManager>
{
    [SerializeField] private InfrastructurePreviewHandler previewHandler;
    
    private InfrastructurePreview currentPreview;

    private Infrastructure currentSelectedStructure;

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

    public static void DeleteInfrastructure(Infrastructure toDelete)
    {
        if (toDelete == instance.currentSelectedStructure)
        {
            UnselectInfrastructure();
        }
        toDelete.RemoveObject();
    }


    /// <summary>
    /// Gère l'intéraction avec une structure.
    /// </summary>
    /// <param name="interactedStructure">L'Infrastructure qui a été cliqué.</param>
    public static void InteractWithStructure(InfrastructureType tool, Infrastructure interactedStructure)
    {
        switch (tool)
        {
            case InfrastructureType.None:
                instance.SelectInfrastructure(interactedStructure);
                break;
            case InfrastructureType.DeleteStructure:
                DeleteInfrastructure(interactedStructure);
                break;
            case InfrastructureType.PathTools:
                break;
        }
    }

    /// <summary>
    /// Sélectionne l'Infrastructure.
    /// </summary>
    /// <param name="selectedStructure">L'Infrastructure à sélectionner.</param>
    private void SelectInfrastructure(Infrastructure selectedStructure)
    {
        if (selectedStructure != currentSelectedStructure)
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
}
