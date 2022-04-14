using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest Objective", menuName = "Quest/Quest Objective/PlaceStructure")]
public class QST_OBJ_PlaceStructure : QST_Objective
{
    [SerializeField] private InfrastructureData structure;

    public InfrastructureData Structure => structure;
}
