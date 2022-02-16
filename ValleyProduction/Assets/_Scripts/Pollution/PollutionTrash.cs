using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PollutionTrash : MonoBehaviour
{
    //public float pollutionLevel = 1f;
    public UnityEvent PlayOnThrowed;
    public UnityEvent PlayOnPicked;

    public void Throw(Vector3 throwedPosition)
    {
        transform.position = throwedPosition;
        gameObject.SetActive(true);
        PlayOnThrowed?.Invoke();
    }

    public void PickUp()
    {
        PlayOnPicked?.Invoke();
        PollutionManager.PickUpTrash(this);
        gameObject.SetActive(false);
    }
}
