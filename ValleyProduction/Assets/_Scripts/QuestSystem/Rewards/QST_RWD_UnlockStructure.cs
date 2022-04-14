using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest Reward", menuName = "Quest/Quest Reward/Structure")]
public class QST_RWD_UnlockStructure : QST_Reward
{
    [SerializeField] private List<InfrastructureData> toUnlock;

    public List<InfrastructureData> ToUnlock => toUnlock;
}
