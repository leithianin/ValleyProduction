using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QST_RWDB_QuestStart : QST_RewardBehavior<QST_RWD_QuestStart>
{
    public override void GiveReward(QST_RWD_QuestStart reward)
    {
        VLY_QuestManager.BeginQuest(reward.Quest);
    }
}
