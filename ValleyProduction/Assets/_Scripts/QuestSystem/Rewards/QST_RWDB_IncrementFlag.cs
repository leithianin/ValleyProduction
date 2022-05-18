using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QST_RWDB_IncrementFlag : QST_RewardBehavior<QST_RWD_IncrementFlag>
{
    public override void GiveReward(QST_RWD_IncrementFlag reward)
    {
        VLY_FlagManager.IncrementFlagValue(reward.FlagName, reward.IncrementValue);
    }
}
