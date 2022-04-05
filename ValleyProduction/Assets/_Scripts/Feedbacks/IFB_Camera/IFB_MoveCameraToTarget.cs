using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IFB_MoveCameraToTarget : MonoBehaviour, IFeedbackPlayer
{
    public float targetRadius;
    public float targetAzimuthalAngle;
    public float targetPolarAngle;
    public float speed;

    public GameObject target;

    public void Play()
    {
        CameraManager.MoveCamera(targetRadius, targetAzimuthalAngle, targetPolarAngle, speed);
    }

    private void Update()
    {
        CameraManager.UpdatePositionOrigin(target.transform.position);
    }

    public void SetTarget(GameObject tar)
    {

    }
}
