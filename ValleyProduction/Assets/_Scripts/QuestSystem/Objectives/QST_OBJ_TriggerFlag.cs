using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest Objective", menuName = "Quest/Quest Objective/Trigger Flag")]
public class QST_OBJ_TriggerFlag : QST_Objective
{
    [SerializeField] private string flagToTrigger;

    public string TriggeredFlag => flagToTrigger;
}
