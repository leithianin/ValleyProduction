using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IFB_MoveCameraToTarget : MonoBehaviour, IFeedbackPlayer
{
    public GameObject target;
    private Camera cam;
    private Transform origin;

    private void Start()
    {
        cam = Camera.main;
        origin = cam.transform.parent.GetChild(0).transform;
    }

    public void Play()
    {
        origin.position = target.transform.position;
    }
}
