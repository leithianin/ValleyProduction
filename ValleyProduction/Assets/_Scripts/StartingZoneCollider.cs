using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StartingZoneCollider : MonoBehaviour
{
    [SerializeField] private UnityEvent OnAddSpawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        IST_PathPoint pathpoint = other.gameObject.GetComponent<IST_PathPoint>();

        if (pathpoint != null)
        {
            VisitorManager.GetVisitorSpawnPoints.Add(pathpoint);
            PathManager.SpawnPoints.Add(pathpoint);
        }
    }
}
