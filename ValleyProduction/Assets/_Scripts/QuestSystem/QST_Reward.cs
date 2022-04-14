using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QST_Reward : ScriptableObject
{
    [SerializeField] protected QST_UI_Reward rewardDisplay;

    public QST_UI_Reward RewardDisplay => rewardDisplay;
}
