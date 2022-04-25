using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NodePathData
{
    public LandmarkType landmark;
    public float distanceFromLandmark = -1f;
    public PathNode parent;

    public bool linkedToLandmark;

    public NodePathData()
    {

    }

    public NodePathData(LandmarkType nLandmark)
    {
        landmark = nLandmark;
    }
}
