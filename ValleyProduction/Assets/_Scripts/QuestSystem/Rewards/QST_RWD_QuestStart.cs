using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest Reward", menuName = "Valley/Quest/Quest Reward/Start Quest")]
public class QST_RWD_QuestStart : QST_Reward
{
    [SerializeField] private VLY_Quest quest;

    public VLY_Quest Quest => quest;
}
