using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LandmarkType
{
    None,
    Spawn,
    Ruin
}

public class CPN_IsLandmark : VLY_Component
{
    [SerializeField] private LandmarkType type;

    public LandmarkType Type => type;

    public List<IST_PathPoint> pathPointNextTo = new List<IST_PathPoint>();
}
