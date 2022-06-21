using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QST_Objective : ScriptableObject
{
    public Action<QuestObjectiveState> OnUpdateState;

    [SerializeField, TextArea(2,3)] private string description;
    [SerializeField, TextArea(2,3)] private string descriptionfr;

    [Header("Dev")]
    [SerializeField] protected QuestObjectiveState state;

    public QuestObjectiveState State => state;

    public string Description => description;
    public string Descriptionfr => descriptionfr;
    /// <summary>
    /// Met à jour l'état de l'objectif
    /// </summary>
    /// <param name="nState">Le nouvel état.</param>
    public void UpdateState(QuestObjectiveState nState)
    {
        state = nState;
        OnUpdateState?.Invoke(state);

        if(state == QuestObjectiveState.Completed)
        {
            OnUpdateState = null;
        }
    }

    /// <summary>
    /// Reset les données Runtime de l'objectif.
    /// </summary>
    public void Reset()
    {
        state = QuestObjectiveState.Pending;
        OnUpdateState = null;
    }
}
