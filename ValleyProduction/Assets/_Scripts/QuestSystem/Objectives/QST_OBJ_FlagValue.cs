using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest Objective", menuName = "Valley/Quest/Quest Objective/Flag Value")]
public class QST_OBJ_FlagValue : QST_Objective
{
    [SerializeField] private string wantedFlag;
    [SerializeField] private int wantedValue;

    public string Flag => wantedFlag;
    public int Value => wantedValue;
}
