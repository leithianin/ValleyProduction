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

    [SerializeField] private VisitorScriptable[] visitorTypes;

    [SerializeField] private List<VisitorBehavior> visitorPool;

    private float nextSpawnTime = 5f;

    public static List<IST_PathPoint> GetVisitorSpawnPoints => instance.visitorSpawnPoints;

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
        VisitorBehavior newVisitor = GetAvailableVisitor();

        Vector2 rng = UnityEngine.Random.insideUnitCircle * spawnDistanceFromSpawnPoint;
        IST_PathPoint wantedSpawn = visitorSpawnPoints[Random.Range(0, visitorSpawnPoints.Count)];
        Vector3 spawnPosition = wantedSpawn.transform.position + new Vector3(rng.x, 0, rng.y);

        NavMeshHit hit;

        if (newVisitor != null && NavMesh.SamplePosition(spawnPosition, out hit, 5f, NavMesh.AllAreas))
        {
            VisitorScriptable visitorType = ChooseVisitorType();

            PathData chosenPath = ChoosePath(visitorType, wantedSpawn);

            if (chosenPath != null)
            {
                newVisitor.SetVisitor(wantedSpawn, spawnPosition, visitorType, chosenPath);
            }
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

    public PathData ChoosePath(VisitorScriptable visitorType, IST_PathPoint spawnPoint)
    {
        List<PathData> allPath = PathManager.GetAllUsablePath(spawnPoint);
        List<PathData> possiblePath = new List<PathData>();

        for(int i = 0; i < allPath.Count; i++)
        {
            //Check des Priorités des Paths
            possiblePath.Add(allPath[i]);
        }

        if (possiblePath.Count > 0)
        {
            return possiblePath[Random.Range(0, possiblePath.Count)];
        }
        else
        {
            return null;
        }
    }
}
