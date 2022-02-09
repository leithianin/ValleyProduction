using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollutionTrash : MonoBehaviour
{
    public float pollutionLevel = 1f;

    public void Throw(Vector3 throwedPosition)
    {
        transform.position = throwedPosition;
        gameObject.SetActive(true);
    }

    public void PickUp()
    {
        PollutionManager.PickUpTrash(this);
        gameObject.SetActive(false);
    }
}
