using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QST_OBJB_TriggerFlag : QST_ObjectiveBehavior<QST_OBJ_TriggerFlag>
{
    private Dictionary<QST_OBJ_TriggerFlag, Action> pendingObjectiveWithHandler = new Dictionary<QST_OBJ_TriggerFlag, Action>();

    protected override void OnCompleteObjective(QST_OBJ_TriggerFlag objective)
    {
        VLY_FlagManager.RemoveTriggerFlagListener(objective.TriggeredFlag, pendingObjectiveWithHandler[objective]);

        pendingObjectiveWithHandler.Remove(objective);

        Debug.Log(objective + " ended.");
    }

    protected override void OnRefreshObjective(QST_OBJ_TriggerFlag objective)
    {
        
    }

    protected override void OnStartObjective(QST_OBJ_TriggerFlag objective)
    {
        Action actionHandler = () => CheckFlag(objective);

        pendingObjectiveWithHandler.Add(objective, actionHandler);

        VLY_FlagManager.AddTriggerFlagListener(objective.TriggeredFlag, actionHandler);
    }

    private void CheckFlag(QST_OBJ_TriggerFlag objective)
    {
        AskCompleteObjective(objective);
    }
}
