using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest Reward", menuName = "Quest/Quest Reward/Increment Flag")]
public class QST_RWD_IncrementFlag : QST_Reward
{
    [SerializeField] private string flagName;
    [SerializeField] private int incrementValue;

    public string FlagName => flagName;
    public int IncrementValue => incrementValue;
}
