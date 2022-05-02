using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest Reward", menuName = "Quest/Quest Reward/Start Quest")]
public class QST_RWD_Flag : QST_Reward
{
    [SerializeField] private string flagName;
    [SerializeField] private int incrementValue;

    public string FlagName => flagName;
    public int IncrementValue => incrementValue;
}
