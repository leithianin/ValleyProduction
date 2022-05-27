using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest Reward", menuName = "Valley/Quest/Quest Reward/Visitor Type")]
public class QST_RWD_VisitorType : QST_Reward
{
    [SerializeField] private VisitorScriptable visitor;

    public VisitorScriptable Visitor => visitor;
}
