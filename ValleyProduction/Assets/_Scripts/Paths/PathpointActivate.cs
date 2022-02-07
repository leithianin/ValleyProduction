using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathpointActivate : MonoBehaviour
{
    public float timer;

    private void OnEnable()
    {
        TimerManager.CreateRealTimer(timer, Activate);
    }

    private void Activate()
    {
        gameObject.layer = 0;
    }
}
