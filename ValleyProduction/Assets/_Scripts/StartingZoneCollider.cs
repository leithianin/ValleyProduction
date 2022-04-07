using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StartingZoneCollider : MonoBehaviour
{
    [SerializeField] private UnityEvent OnAddSpawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Si je vois ce message, c'est que ce script est utilis�");
        IST_PathPoint pathpoint = other.gameObject.GetComponent<IST_PathPoint>();

        if (pathpoint != null)
        {
            if (!IsPathAlreadyHaveASpawnPoint())
            {
                PathManager.isOnSpawn?.Invoke(true);
                PathManager.SpawnPoints.Add(pathpoint);
            }
        }
    }

    private bool IsPathAlreadyHaveASpawnPoint()
    {
        foreach(IST_PathPoint ist_pp in PathManager.GetCurrentPathpointList)
        {
            foreach(IST_PathPoint ist_spawn in PathManager.SpawnPoints)
            {
                if(ist_pp == ist_spawn)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
