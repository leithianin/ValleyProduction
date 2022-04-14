using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QST_ObjectiveBehavior
{
    public abstract Type GetObjectiveType();

    public abstract void SetObjectiveStatus(QST_Objective objective, QuestObjectiveState state);
}

public abstract class QST_ObjectiveBehavior<T> : QST_ObjectiveBehavior where T : QST_Objective
{
    public override Type GetObjectiveType()
    {
        return typeof(T);
    }

    /// <summary>
    /// Appelé quand la valeur utilisé par l'objectif est mise à jour.
    /// </summary>
    /// <param name="objective">Référence à l'objectif mise à jour.</param>
    protected abstract void OnRefreshObjective(T objective);

    /// <summary>
    /// Appelé quand la quête est lancée.
    /// </summary>
    /// <param name="objective"></param>
    protected abstract void OnStartObjective(T objective);

    /// <summary>
    /// Appelé quand la quête est terminée.
    /// </summary>
    /// <param name="objective"></param>
    protected abstract void OnCompleteObjective(T objective);

    /// <summary>
    /// Demande à compléter un objectif (Fait le lien avec le Manager)
    /// </summary>
    /// <param name="objective">L'objectif à compléter.</param>
    protected void AskCompleteObjective(T objective)
    {
        VLY_QuestManager.SetObjectiveStatus(objective, QuestObjectiveState.Completed);
    }

    /// <summary>
    /// Set l'objectif demandé à l'état voulut si possible.
    /// </summary>
    /// <param name="objective">L'objectif à mettre à jour.</param>
    /// <param name="state">Le nouvel état de l'objectif.</param>
    public override void SetObjectiveStatus(QST_Objective objective, QuestObjectiveState state)
    {
        T obj = objective as T;

        if (obj != null)
        {
            if (obj.State < state) // On vérifie que l'état de l'objectif est bien un état précédent l'état voulut.
            {
                switch (state)
                {
                    case QuestObjectiveState.Started:
                        OnStartObjective(obj);
                        break;
                    case QuestObjectiveState.Failed:
                        break;
                    case QuestObjectiveState.Completed:
                        OnCompleteObjective(obj);
                        break;
                }

                obj.UpdateState(state);
            }
        }
    }
}
