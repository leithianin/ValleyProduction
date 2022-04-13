using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QST_RWDB_UnlockStructure : QST_RewardBehavior<QST_RWD_UnlockStructure>
{
    public override void GiveReward(QST_RWD_UnlockStructure reward)
    {
        UIManager.UnlockStructure(reward.ToUnlock);
    }
}
