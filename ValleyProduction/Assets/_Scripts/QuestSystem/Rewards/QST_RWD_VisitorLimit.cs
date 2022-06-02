using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest Reward", menuName = "Valley/Quest/Quest Reward/Visitor Limit")]
public class QST_RWD_VisitorLimit : QST_Reward
{
    [SerializeField] private int limitToAdd;

    public int LimitToAdd => limitToAdd;
}
