using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QST_RWDB_VisitorType : QST_RewardBehavior<QST_RWD_VisitorType>
{
    public override void GiveReward(QST_RWD_VisitorType reward)
    {
        VisitorManager.AddVisitorType(reward.Visitor);
    }
}
