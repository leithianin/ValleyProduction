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
    private bool isRotate;

    public void Play()
    {
        CameraManager.SetTarget(target.transform);
        //Si isRotate play func with bool isRotate
        CameraManager.MoveCamera(targetRadius, targetAzimuthalAngle, targetPolarAngle, speed);
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
