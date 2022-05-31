using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IFB_MoveCameraToTarget : MonoBehaviour, IFeedbackPlayer
{
    public UnityEvent OnEnd;

    [SerializeField] private float targetRadius;
    [SerializeField] private float targetAzimuthalAngle;
    [SerializeField] private float targetPolarAngle;
    [SerializeField] private float speed;

    public GameObject target;
    public bool isRotate;

    public void Play()
    {
        CameraManager.SetTargetWithSpeed(target.transform, speed);
        CameraManager.OnCameraMoveEnd += PlayOnEnd;

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

    public void PlayOnEnd()
    {
        CameraManager.OnCameraMoveEnd -= PlayOnEnd;
        OnEnd?.Invoke();
    }
}
