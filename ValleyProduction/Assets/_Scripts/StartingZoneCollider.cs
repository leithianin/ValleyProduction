using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StartingZoneCollider : MonoBehaviour
{
    //public UnityEvent<IST_PathPoint> OnPlacePathpoint;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject);
        IST_PathPoint pathpoint = other.gameObject.GetComponent<IST_PathPoint>();

        if (pathpoint != null)
        {
            VisitorManager.GetVisitorSpawnPoints.Add(pathpoint);
        }
    }
}
