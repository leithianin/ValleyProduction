using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest Reward", menuName = "Quest/Quest Reward/Trigger Flag")]
public class QST_RWD_TriggerFlag : QST_Reward
{
    [SerializeField] private string flagName;

    public string FlagName => flagName;
}
