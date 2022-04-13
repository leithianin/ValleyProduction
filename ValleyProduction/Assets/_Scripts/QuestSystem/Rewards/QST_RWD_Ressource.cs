using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest Reward", menuName = "Quest/Quest Reward/Ressource")]
public class QST_RWD_Ressource : QST_Reward
{
    [SerializeField] private VLY_GlobalData ressourceType;
    [SerializeField] private float amountToAdd;

    public VLY_GlobalData RessourceType => ressourceType;

    public float Amount => amountToAdd;
}
