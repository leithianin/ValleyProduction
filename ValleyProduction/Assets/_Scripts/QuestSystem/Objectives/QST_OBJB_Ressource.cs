using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QST_OBJB_Ressource : QST_ObjectiveBehavior<QST_OBJ_Ressource>
{
    private List<QST_OBJ_Ressource> pendingObjectives = new List<QST_OBJ_Ressource>();

    protected override void OnCompleteObjective(QST_OBJ_Ressource objective)
    {
        pendingObjectives.Remove(objective);
        if (pendingObjectives.Count <= 0)
        {
            objective.Ressource.OnValueChange -= OnUpdateValues;
        }
        Debug.Log(objective + " ended.");
    }

    protected override void OnRefreshObjective(QST_OBJ_Ressource objective) // CODE REVIEW : Besoin de l'avoir dans la classe parent ?
    {
        if(objective.RessourceAmount < objective.Ressource.Value)
        {
            AskCompleteObjective(objective);
        }
    }

    protected override void OnStartObjective(QST_OBJ_Ressource objective)
    {
        pendingObjectives.Add(objective);
        if (pendingObjectives.Count < 2)
        {
            objective.Ressource.OnValueChange += OnUpdateValues;
        }
    }

    private void OnUpdateValues(float f)
    {
        for(int i = 0; i < pendingObjectives.Count; i++)
        {
            OnRefreshObjective(pendingObjectives[i]);
        }
    }
}
