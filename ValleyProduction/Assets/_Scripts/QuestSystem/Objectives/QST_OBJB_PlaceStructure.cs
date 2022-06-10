using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QST_OBJB_PlaceStructure : QST_ObjectiveBehavior<QST_OBJ_PlaceStructure>
{
    private List<QST_OBJ_PlaceStructure> pendingObjectives = new List<QST_OBJ_PlaceStructure>();

    protected override void OnCompleteObjective(QST_OBJ_PlaceStructure objective)
    {
        pendingObjectives.Remove(objective);
        if (pendingObjectives.Count <= 0)
        {
            InfrastructureManager.OnPlaceInfrastructure -= CheckPlacedStructure;
        }
        Debug.Log(objective + " ended.");
    }

    protected override void OnRefreshObjective(QST_OBJ_PlaceStructure objective)
    {
        
    }

    protected override void OnStartObjective(QST_OBJ_PlaceStructure objective)
    {
        if (pendingObjectives.Count == 0)
        {
            InfrastructureManager.OnPlaceInfrastructure += CheckPlacedStructure;
        }
        pendingObjectives.Add(objective);

    }

    private void CheckPlacedStructure(Infrastructure placeStructure)
    {
        for (int i = 0; i < pendingObjectives.Count; i++)
        {
            if(placeStructure.Data == pendingObjectives[i].Structure)
            {
                AskCompleteObjective(pendingObjectives[i]);
            }
        }
    }
}
