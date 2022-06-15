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
    [SerializeField] private float duration;

    public GameObject target;
    public bool isRotate;

    public void Play()
    {
        if (isRotate)
        {
            CameraManager.MoveCamera(target.transform, targetRadius, targetAzimuthalAngle, targetPolarAngle, duration, isRotate);
            CameraManager.OnCameraMoveEnd += PlayOnEnd;
            CameraManager.OnCameraMove += StopFocus;
        }
        else
        {
            CameraManager.SetTargetWithSpeed(target.transform, 1 / duration);
        }
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
