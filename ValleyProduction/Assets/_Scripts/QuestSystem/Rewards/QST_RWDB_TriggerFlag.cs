using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QST_RWDB_TriggerFlag : QST_RewardBehavior<QST_RWD_TriggerFlag>
{
    public override void GiveReward(QST_RWD_TriggerFlag reward)
    {
        VLY_FlagManager.TriggerFlag(reward.FlagName);
    }
}
