using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest Objective", menuName = "Valley/Quest/Quest Objective/Ressource")]
public class QST_OBJ_Ressource : QST_Objective
{
    [SerializeField] private float wantedRessourceAmount;
    [SerializeField] private VLY_GlobalData wantedRessource;

    public float RessourceAmount => wantedRessourceAmount;

    public VLY_GlobalData Ressource => wantedRessource;
}
