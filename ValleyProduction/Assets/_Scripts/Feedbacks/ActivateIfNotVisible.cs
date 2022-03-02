using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateIfNotVisible : MonoBehaviour
{
    public GameObject target;                        //If target not visible, activate this gameobject
    public Image image;
    public Camera cam;

    private void Update()
    {
        if(cam.WorldToViewportPoint(target.transform.position).z > 0 && IsBetween(cam.WorldToViewportPoint(target.transform.position).x, 0, 1) && IsBetween(cam.WorldToViewportPoint(target.transform.position).y, 0, 1))
        {
            image.enabled = false;
        }
        else
        {
            image.enabled = true;
        }
    }

    public bool IsBetween(float value, float min, float max)
    {
        return (value >= Math.Min(min, max) && value <= Math.Max(min, max));
    }
}
