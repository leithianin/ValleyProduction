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
    public float speed;

    public bool isRotating;
    public bool clockwise;
    public float rotationSpeed;
    public bool cinematic;

    public bool useCustomDuration;
    public float duration;

    public Vector3 cameraOriginPosition;
    public Vector3 travelPosition;

}
