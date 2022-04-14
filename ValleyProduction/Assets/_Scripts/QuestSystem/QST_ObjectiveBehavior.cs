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
    /// Appel� quand la valeur utilis� par l'objectif est mise � jour.
    /// </summary>
    /// <param name="objective">R�f�rence � l'objectif mise � jour.</param>
    protected abstract void OnRefreshObjective(T objective);

    /// <summary>
    /// Appel� quand la qu�te est lanc�e.
    /// </summary>
    /// <param name="objective"></param>
    protected abstract void OnStartObjective(T objective);

    /// <summary>
    /// Appel� quand la qu�te est termin�e.
    /// </summary>
    /// <param name="objective"></param>
    protected abstract void OnCompleteObjective(T objective);

    /// <summary>
    /// Demande � compl�ter un objectif (Fait le lien avec le Manager)
    /// </summary>
    /// <param name="objective">L'objectif � compl�ter.</param>
    protected void AskCompleteObjective(T objective)
    {
        VLY_QuestManager.SetObjectiveStatus(objective, QuestObjectiveState.Completed);
    }

    /// <summary>
    /// Set l'objectif demand� � l'�tat voulut si possible.
    /// </summary>
    /// <param name="objective">L'objectif � mettre � jour.</param>
    /// <param name="state">Le nouvel �tat de l'objectif.</param>
    public override void SetObjectiveStatus(QST_Objective objective, QuestObjectiveState state)
    {
        T obj = objective as T;

        if (obj != null)
        {
            if (obj.State < state) // On v�rifie que l'�tat de l'objectif est bien un �tat pr�c�dent l'�tat voulut.
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
