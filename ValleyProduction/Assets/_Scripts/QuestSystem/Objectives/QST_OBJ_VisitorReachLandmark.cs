using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest Objective", menuName = "Valley/Quest/Quest Objective/Visitor Reach Landmark")]
public class QST_OBJ_VisitorReachLandmark : QST_Objective
{
    [SerializeField] private VisitorScriptable visitorType;
    [SerializeField] private LandmarkType landmarkTarget;
    [Header("Visitor data wanted")]
    [SerializeField, Tooltip("Check if the visitor has more staisfaction than this value.")] private float satisfaction;
    [SerializeField, Tooltip("Check if the visitor has more stamina than this value.")] private float stamina;

    public VisitorScriptable Visitor => visitorType;
    public LandmarkType Landmark => landmarkTarget;
    public float Satisfaction => satisfaction;
    public float Stamina => stamina;
}
