using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QST_RewardBehavior
{
    public abstract Type GetRewardType();

    public abstract void GiveReward(QST_Reward reward);
}

public abstract class QST_RewardBehavior<T> : QST_RewardBehavior where T : QST_Reward
{
    public override Type GetRewardType()
    {
        return typeof(T);
    }

    /// <summary>
    /// Appel� pour donner les r�compenses d'une qu�te.
    /// </summary>
    /// <param name="reward">Les r�compenses � donner.</param>
    public abstract void GiveReward(T reward);

    public override void GiveReward(QST_Reward reward)
    {
        GiveReward(reward as T);
    }
}
