using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QST_Objective : ScriptableObject
{
    public Action<QuestObjectiveState> OnUpdateState;

    [SerializeField] protected QuestObjectiveState state;

    [SerializeField] private string description;

    public QuestObjectiveState State => state;

    public string Description => description;

    /// <summary>
    /// Met � jour l'�tat de l'objectif
    /// </summary>
    /// <param name="nState">Le nouvel �tat.</param>
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
    /// Reset les donn�es Runtime de l'objectif.
    /// </summary>
    public void Reset()
    {
        state = QuestObjectiveState.Pending;
        OnUpdateState = null;
    }
}
