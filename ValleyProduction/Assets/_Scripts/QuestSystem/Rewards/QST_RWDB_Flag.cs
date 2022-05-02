using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QST_RWDB_Flag : QST_RewardBehavior<QST_RWD_Flag>
{
    public override void GiveReward(QST_RWD_Flag reward)
    {
        VLY_FlagManager.IncrementFlagValue(reward.FlagName, reward.IncrementValue);
    }
}
