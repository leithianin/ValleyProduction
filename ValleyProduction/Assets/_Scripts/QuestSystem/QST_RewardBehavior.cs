using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QST_RewardBehavior<T> where T : QST_Reward
{
    /// <summary>
    /// Appelé pour donner les récompenses d'une quête.
    /// </summary>
    /// <param name="reward">Les récompenses à donner.</param>
    public abstract void GiveReward(T reward);

    /// <summary>
    /// Appelé quand la quête est lancée.
    /// </summary>
    /// <param name="quest"></param>
    protected abstract void StartQuest(T quest);

    /// <summary>
    /// Appelé quand la quête est terminée.
    /// </summary>
    /// <param name="quest"></param>
    protected abstract void EndQuest(T quest);
}
