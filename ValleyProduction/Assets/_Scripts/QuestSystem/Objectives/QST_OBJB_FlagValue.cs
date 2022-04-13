using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QST_OBJB_FlagValue : QST_ObjectiveBehavior<QST_OBJ_FlagValue>
{
    private List<QST_OBJ_FlagValue> pendingObjectives = new List<QST_OBJ_FlagValue>();

    protected override void OnCompleteObjective(QST_OBJ_FlagValue objective)
    {
        pendingObjectives.Remove(objective);
        if (pendingObjectives.Count <= 0)
        {
            VLY_FlagManager.OnUpdateFlag -= CheckFlag;
        }
        Debug.Log(objective + " ended.");
    }

    protected override void OnRefreshObjective(QST_OBJ_FlagValue objective)
    {
        
    }

    protected override void OnStartObjective(QST_OBJ_FlagValue objective)
    {
        if (VLY_FlagManager.GetFlagValue(objective.Flag) >= objective.Value)
        {
            Debug.Log(objective);
            AskCompleteObjective(objective);
        }
        else
        {
            pendingObjectives.Add(objective);
            if (pendingObjectives.Count < 2)
            {
                VLY_FlagManager.OnUpdateFlag += CheckFlag;
            }
        }
    }

    private void CheckFlag(string flagName, int flagValue)
    {
        for(int i = 0; i < pendingObjectives.Count; i++)
        {
            if (flagName == pendingObjectives[i].Flag && flagValue >= pendingObjectives[i].Value)
            {
                AskCompleteObjective(pendingObjectives[i]);
            }
        }
    }
}
