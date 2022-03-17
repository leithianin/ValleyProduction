using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IFB_ZoomCamera : MonoBehaviour, IFeedbackPlayer
{
    public SphericalTransform camSphericalTr;

    private float zoom = 0f;

    public void Play()
    {
        camSphericalTr.SetLength(zoom);
    }

    public void Play(float zoomValue)
    {
        zoom = zoomValue;
        Play();
    }
}
