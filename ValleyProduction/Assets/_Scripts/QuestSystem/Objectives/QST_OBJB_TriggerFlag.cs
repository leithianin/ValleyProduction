using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QST_OBJB_TriggerFlag : QST_ObjectiveBehavior<QST_OBJ_TriggerFlag>
{
    private List<QST_OBJ_TriggerFlag> pendingObjectives = new List<QST_OBJ_TriggerFlag>();

    protected override void OnCompleteObjective(QST_OBJ_TriggerFlag objective)
    {
        pendingObjectives.Remove(objective);
        if (pendingObjectives.Count <= 0)
        {
            VLY_FlagManager.OnTriggerFlag -= CheckFlag;
        }
        //Debug.Log(objective + " ended.");
    }

    protected override void OnRefreshObjective(QST_OBJ_TriggerFlag objective)
    {
        
    }

    protected override void OnStartObjective(QST_OBJ_TriggerFlag objective)
    {
        pendingObjectives.Add(objective);
        if (pendingObjectives.Count < 2)
        {
            VLY_FlagManager.OnTriggerFlag += CheckFlag;
        }
    }

    private void CheckFlag(string flagName)
    {
        for (int i = 0; i < pendingObjectives.Count; i++)
        {
            if (flagName == pendingObjectives[i].TriggeredFlag)
            {
                AskCompleteObjective(pendingObjectives[i]);
            }
        }
    }
}
