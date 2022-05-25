using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IFB_MoveCameraToTargetOnBoarding : MonoBehaviour, IFeedbackPlayer
{
    private Transform target;
    public int speed = 20;

    public void Play()
    {
        CameraManager.SetTargetWithSpeed(target, speed);
    }

    public void Play(Transform _target)
    {
        target = _target;
        Play();
    }
}
