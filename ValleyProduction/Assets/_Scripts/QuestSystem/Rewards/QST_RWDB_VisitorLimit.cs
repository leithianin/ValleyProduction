using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QST_RWDB_VisitorLimit : QST_RewardBehavior<QST_RWD_VisitorLimit>
{
    public override void GiveReward(QST_RWD_VisitorLimit reward)
    {
        VisitorManager.AddVisitorLimit(reward.LimitToAdd);
    }
}
