using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VisitorManager : VLY_Singleton<VisitorManager>
{
    [SerializeField] private List<IST_PathPoint> visitorSpawnPoints;

    [SerializeField] private float timeBetweenSpawn = 10f;
    [SerializeField] private Vector2Int visitorToSpawnNb = Vector2Int.zero;
    [SerializeField] private float spawnDistanceFromSpawnPoint = 0.8f;
    [SerializeField] private int maxSpawn = 100;

    [SerializeField] private VisitorScriptable[] visitorTypes;

    // CODE REVIEW : Voir si on peut pas faire le choix du Landmark autre part.
    [SerializeField] private InterestPoint[] landmarks;

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
            newVisitor.SetVisitor(wantedSpawn, spawnPosition, ChooseVisitorType());

            //Choix objectif
            InterestPoint objective = ChooseObjective(newVisitor.VisitorType.LandmarkTarget[0]);
        }
    }

    public static void DeleteVisitor(VisitorBehavior toDelete)
    {
        toDelete.UnsetVisitor();
    }

    private VisitorScriptable ChooseVisitorType()
    {
        return visitorTypes[Random.Range(0, visitorTypes.Length)];
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

    private InterestPoint ChooseObjective(InterestPointType wantedType)
    {
        for (int i = 0; i < landmarks.Length; i++)
        {
            if (landmarks[i].Type == wantedType)
            {
                return landmarks[i];
            }
        }

        return null;
    }

    public void ChoosePath()
    {
        // TO DO
    }
}
