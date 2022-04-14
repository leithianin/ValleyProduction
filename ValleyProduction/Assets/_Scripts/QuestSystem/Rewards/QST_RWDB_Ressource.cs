using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QST_RWDB_Ressource : QST_RewardBehavior<QST_RWD_Ressource>
{
    public override void GiveReward(QST_RWD_Ressource reward)
    {
        reward.RessourceType.AskChangeValue(reward.Amount);
    }
}
