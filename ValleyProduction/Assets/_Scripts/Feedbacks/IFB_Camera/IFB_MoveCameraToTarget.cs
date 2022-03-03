using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IFB_MoveCameraToTarget : MonoBehaviour, IFeedbackPlayer
{
    public GameObject target;
    public SphericalTransform camSphericalTr;

    private int speed = 0;

    public void Play()
    {
        StartCoroutine(camSphericalTr.MoveCameraOriginToCustomTarget(target.transform, speed));
    }

    public void Play(int newSpeed)
    {
        speed = newSpeed;
        Play();
    }
}
