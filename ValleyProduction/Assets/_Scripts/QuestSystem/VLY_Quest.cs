using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest Quest", menuName = "Quest/Create Quest")]
public class VLY_Quest : ScriptableObject
{
    public QuestObjectiveState state;

    [SerializeField] private List<QST_Objective> objectives;

    [SerializeField] private List<QST_Reward> rewards;

    public List<QST_Objective> Objectives => objectives;

    public List<QST_Reward> Rewards => rewards;

    /// <summary>
    /// Reset les données Runtime de la quête
    /// </summary>
    public void Reset()
    {
        state = QuestObjectiveState.Pending;
    }
}
