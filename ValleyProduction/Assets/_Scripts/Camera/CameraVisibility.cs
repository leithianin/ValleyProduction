using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraVisibility
{
    public static bool IsObjectVisible(this UnityEngine.Camera @this, Bounds bounds)
    {
        return GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(@this), bounds);
    }
}

