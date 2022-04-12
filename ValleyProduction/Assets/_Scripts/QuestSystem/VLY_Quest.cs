using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest Quest", menuName = "Quest/Create Quest")]
public class VLY_Quest : ScriptableObject
{
    [Serializable]
    public class QST_ObjectiveStage
    {
        public QuestObjectiveState State;

        public List<QST_Objective> Objectives;
    }

    public QuestObjectiveState state;

    private int currentStage;

    [SerializeField] private List<QST_ObjectiveStage> stages;

    [SerializeField] private List<QST_Reward> rewards;

    public List<QST_ObjectiveStage> Stages => stages;

    public List<QST_Reward> Rewards => rewards;

    public List<QST_Objective> GetCurrentStageObjectives => stages[currentStage].Objectives;

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
