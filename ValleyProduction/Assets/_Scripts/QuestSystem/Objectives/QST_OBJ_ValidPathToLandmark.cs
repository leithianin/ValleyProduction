using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest Objective", menuName = "Quest/Quest Objective/Valid path to Landmark")]
public class QST_OBJ_ValidPathToLandmark : QST_Objective
{
    [SerializeField] private LandmarkType landmarkTarget;

    public LandmarkType Landmark => landmarkTarget;
}
