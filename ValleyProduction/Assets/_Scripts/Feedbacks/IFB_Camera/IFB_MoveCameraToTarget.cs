using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IFB_MoveCameraToTarget : MonoBehaviour, IFeedbackPlayer
{
    [SerializeField] private float targetRadius;
    [SerializeField] private float targetAzimuthalAngle;
    [SerializeField] private float targetPolarAngle;
    [SerializeField] private float speed;

    public GameObject target;
    public bool isRotate;

    public void Play()
    {
            CameraManager.SetTargetWithSpeed(target.transform, speed);


            //CameraManager.SetTarget(target.transform);
            //CameraManager.SetCameraPosition(target.transform.position);
            //Si isRotate play func with bool isRotate
            CameraManager.MoveCamera(targetRadius, targetAzimuthalAngle, targetPolarAngle, speed, isRotate);
            CameraManager.OnCameraMove += StopFocus;
    }

    public void SetTarget(GameObject tar)
    {
        target = tar;
    }

    public void StopFocus()
    {
        CameraManager.OnCameraMove -= StopFocus;
        CameraManager.SetTarget(null);
    }
}
