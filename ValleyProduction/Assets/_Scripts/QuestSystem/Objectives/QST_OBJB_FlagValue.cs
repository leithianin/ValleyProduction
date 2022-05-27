using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QST_OBJB_FlagValue : QST_ObjectiveBehavior<QST_OBJ_FlagValue>
{
    private Dictionary<QST_OBJ_FlagValue, Action<int>> pendingObjectiveWithHandler = new Dictionary<QST_OBJ_FlagValue, Action<int>>();

    protected override void OnCompleteObjective(QST_OBJ_FlagValue objective)
    {
        if (pendingObjectiveWithHandler.ContainsKey(objective))
        {
            VLY_FlagManager.RemoveIncrementFlagListener(objective.Flag, pendingObjectiveWithHandler[objective]);
        }

        pendingObjectiveWithHandler.Remove(objective);

        //Debug.Log(objective + " ended.");
    }

    protected override void OnRefreshObjective(QST_OBJ_FlagValue objective)
    {
        
    }

    protected override void OnStartObjective(QST_OBJ_FlagValue objective)
    {
        if (VLY_FlagManager.GetFlagValue(objective.Flag) >= objective.Value)
        {
            //Debug.Log("End begin flag : " + objective);
            TimerManager.CreateRealTimer(Time.deltaTime, () => AskCompleteObjective(objective));
        }
        else
        {
            Action<int> actionHandler = (value) => CheckFlag(objective, value);

            pendingObjectiveWithHandler.Add(objective, actionHandler);
            VLY_FlagManager.AddIncrementFlagListener(objective.Flag, actionHandler);

        }
    }

    private void CheckFlag(QST_OBJ_FlagValue objective, int flagValue)
    {
        if (flagValue >= objective.Value)
        {
            AskCompleteObjective(objective);
        }
    }
}
