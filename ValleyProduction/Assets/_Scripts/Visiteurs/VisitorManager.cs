using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VisitorManager : VLY_Singleton<VisitorManager>
{
    [SerializeField] private List<IST_PathPoint> visitorSpawnPoints;

    //Visitor types

    [SerializeField] private float timeBetweenSpawn = 10f;
    [SerializeField] private Vector2Int visitorToSpawnNb = Vector2Int.zero;
    [SerializeField] private float spawnDistanceFromSpawnPoint = 0.8f;

    [SerializeField] private int maxSpawn = 100;

    [SerializeField] private List<VisitorBehavior> visitorPool;

    private float nextSpawnTime = 5f;

    private void Update()
    {
        if(Time.time > nextSpawnTime)
        {
            int spawnNb = Random.Range(visitorToSpawnNb.x, visitorToSpawnNb.y+1);
            for (int i = 0; i < spawnNb; i++)
            {
                if(UsedVisitorNumber() < maxSpawn)
                {
                    SpawnVisitor();
                }
            }

            nextSpawnTime += timeBetweenSpawn;
        }
    }

    private void SpawnVisitor()
    {
        //Check si on peut ou non spawn un visiteur
        VisitorBehavior newVisitor = GetAvailableVisitor();

        Vector2 rng = UnityEngine.Random.insideUnitCircle * spawnDistanceFromSpawnPoint;
        IST_PathPoint wantedSpawn = visitorSpawnPoints[Random.Range(0, visitorSpawnPoints.Count)];
        Vector3 spawnPosition = wantedSpawn.transform.position + new Vector3(rng.x, 0, rng.y);

        NavMeshHit hit;
        if (newVisitor != null && NavMesh.SamplePosition(spawnPosition, out hit, .5f, NavMesh.AllAreas))
        {
            newVisitor.SetVisitor(wantedSpawn, spawnPosition);
        }
    }

    public static void DeleteVisitor(VisitorBehavior toDelete)
    {
        toDelete.UnsetVisitor();
    }

    private VisitorBehavior GetAvailableVisitor()
    {
        for (int i = 0; i < visitorPool.Count; i++)
        {
            if (!visitorPool[i].gameObject.activeSelf)
            {
                return visitorPool[i];
            }
        }
        return null;
    }

    private int UsedVisitorNumber()
    {
        int toReturn = 0;
        for (int i = 0; i < visitorPool.Count; i++)
        {
            if (visitorPool[i].gameObject.activeSelf)
            {
                toReturn++;
            }
        }

        return toReturn;
    }
}
