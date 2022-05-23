using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraData : ScriptableObject
{
    [HideInInspector] public string scene;

    public float radius;
    public float azimuthalAngle;
    public float polarAngle;

    public float verticalOffset;

    public bool isTraveling;
    public bool cinematic;

    public Vector3 cameraOriginPosition;
    public Vector3 travelPosition;

    public float speed;
}
