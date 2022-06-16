using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest Quest", menuName = "Valley/Quest/Create Quest")]
public class VLY_Quest : ScriptableObject
{
    private int currentStage;

    public string questName;

    public string questDescription;

    [SerializeField] private List<QST_ObjectiveStage> stages;

    [SerializeField] private List<QST_Reward> rewards;

    [Header("Dev")]
    public QuestObjectiveState state;

    public string QuestName => questName;

    public List<QST_ObjectiveStage> Stages => stages;

    public List<QST_Reward> Rewards => rewards;

    public List<QST_Objective> GetCurrentStageObjectives()
    {
        for(int i = 0; i < stages.Count; i++)
        {
            if(stages[i].State == QuestObjectiveState.Completed)
            {
                continue;
            }
            else
            {
                return stages[i].Objectives;
            }
        }

        return new List<QST_Objective>();
    }

    /// <summary>
    /// Reset les données Runtime de la quête
    /// </summary>
    public void Reset()
    {
        state = QuestObjectiveState.Pending;

        currentStage = 0;

        for (int j = 0; j < Stages.Count; j++)
        {
            Stages[j].State = QuestObjectiveState.Pending;

            for (int k = 0; k < Stages[j].Objectives.Count; k++)
            {
                Stages[j].Objectives[k].Reset();
            }
        }
    }
}

[Serializable]
public class QST_ObjectiveStage
{
    public QuestObjectiveState State;

    public List<QST_Objective> Objectives;

    public List<string> triggerFlagList;
    public List<string> incrementFlagList;

    public string dialogueID;
}
