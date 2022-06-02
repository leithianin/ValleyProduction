using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QST_RWDB_UnlockStructure : QST_RewardBehavior<QST_RWD_UnlockStructure>
{
    public override void GiveReward(QST_RWD_UnlockStructure reward)
    {
        for (int i = 0; i < reward.ToUnlock.Count; i++)
        {
            UIManager.UnlockStructure(reward.ToUnlock[i]);
        }
    }
}
