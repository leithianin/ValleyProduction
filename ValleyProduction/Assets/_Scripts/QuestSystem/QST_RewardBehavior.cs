using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QST_RewardBehavior<T> where T : QST_Reward
{
    /// <summary>
    /// Appel� pour donner les r�compenses d'une qu�te.
    /// </summary>
    /// <param name="reward">Les r�compenses � donner.</param>
    public abstract void GiveReward(T reward);

    /// <summary>
    /// Appel� quand la qu�te est lanc�e.
    /// </summary>
    /// <param name="quest"></param>
    protected abstract void StartQuest(T quest);

    /// <summary>
    /// Appel� quand la qu�te est termin�e.
    /// </summary>
    /// <param name="quest"></param>
    protected abstract void EndQuest(T quest);
}
