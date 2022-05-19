using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IFB_MoveCameraToTargetOnBoarding : MonoBehaviour, IFeedbackPlayer
{
    public Transform target;
    private int speed = 0;

    public void Play()
    {
        CameraManager.SetTargetWithSpeed(target, speed);
    }

    public void Play(int newSpeed)
    {
        speed = newSpeed;
        Play();
    }
}
