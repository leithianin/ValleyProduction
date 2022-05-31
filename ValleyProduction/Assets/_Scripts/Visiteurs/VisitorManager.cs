using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using UnityEngine.Assertions;

public class VisitorManager : VLY_Singleton<VisitorManager>
{
    [SerializeField] private float timeBetweenSpawn = 10f;
    [SerializeField] private Vector2Int visitorToSpawnNb = Vector2Int.zero;
    [SerializeField] private float spawnDistanceFromSpawnPoint = 0.8f;
    [SerializeField] private int maxSpawn = 100;

    [SerializeField] private List<VisitorScriptable> visitorTypes;

    [Header("Feedbacks")]
    [SerializeField] private UnityEvent<int> OnUpdateVisitorNumber;
    [SerializeField] private UnityEvent<int> OnEnterVisitor;
    [SerializeField] private UnityEvent<int> OnExitVisitor;

    [Header("For Dev")]
    [SerializeField] private List<VisitorBehavior> visitorPool;

    public static Action<bool> isOnDespawn;

    private float nextSpawnTime = 5f;

    [SerializeField] private bool allowVisitorSpawn;

    #region Actions
    public static Action<VisitorBehavior> OnSpawnVisitor;
    public static Action<VisitorBehavior> OnDespawnVisitor;
    public static Action<int> OnChangeVisitorCount;
    #endregion



    private void Start()
    {
        nextSpawnTime = Time.time;
    }

    private void Update()
    {
        if (Time.time > nextSpawnTime && allowVisitorSpawn)
        {
            int spawnNb = UnityEngine.Random.Range(visitorToSpawnNb.x, visitorToSpawnNb.y + 1);
            for (int i = 0; i < spawnNb; i++)
            {
                if (UsedVisitorNumber() < maxSpawn)
                {
                    SpawnVisitor();
                    //UIManager.UpdateNbVisitors(UsedVisitorNumber());
                }
            }

            nextSpawnTime += timeBetweenSpawn;
        }
    }
    
    public static void SetVisitorSpawn(bool doesAllow)
    {
        instance.allowVisitorSpawn = doesAllow;
    }

    /// <summary>
    /// Demande � faire appra�tre un visiteur.
    /// </summary>
    private void SpawnVisitor(VisitorScriptable type = null)
    {
        if (PathManager.SpawnPoints.Count > 0)
        {
            VisitorBehavior newVisitor = GetAvailableVisitor();

            if (newVisitor != null)
            {
                VisitorScriptable visitorType;
                if (type != null)
                {
                    visitorType = type;
                }
                else if(visitorTypes.Count > 0)
                {
                    visitorType = ChooseVisitorType();
                }
                else
                {
                    visitorType = newVisitor.visitorType;
                }

                Vector2 rng = UnityEngine.Random.insideUnitCircle * spawnDistanceFromSpawnPoint;
                IST_PathPoint wantedSpawn = null;

                wantedSpawn = SearchSpawnPoint(visitorType.LandmarksWanted);

                if (wantedSpawn == null)
                {
                    wantedSpawn = SearchSpawnPointWithoutLandmark();
                }

                /*for (int i = 0; i < visitorType.LandmarksWanted.Count; i++)
                {
                    wantedSpawn = SearchSpawnPoint(visitorType.LandmarksWanted[i]);

                    if(wantedSpawn != null)
                    {
                        break;
                    }
                    else if(i == visitorType.LandmarksWanted.Count - 1)
                    {
                        wantedSpawn = SearchSpawnPointWithoutLandmark();
                    }
                }*/

                if (wantedSpawn != null)
                {

                    CPN_IsLandmark visitorObjective = SearchObjective(visitorType, wantedSpawn.Node);

                    Vector3 spawnPosition = wantedSpawn.transform.position + new Vector3(rng.x, 0, rng.y);

                    NavMeshHit hit;

                    if (NavMesh.SamplePosition(spawnPosition, out hit, 5f, NavMesh.AllAreas))
                    {
                        newVisitor.SetVisitor(wantedSpawn, spawnPosition, visitorType, visitorObjective); //CODE REVIEW
                        SetType(newVisitor);

                        OnUpdateVisitorNumber?.Invoke(UsedVisitorNumber());

                        OnSpawnVisitor?.Invoke(newVisitor);
                        OnChangeVisitorCount?.Invoke(UsedVisitorNumber());
                    }
                }
            }
        }
    }

    /// <summary>
    /// Spawn un visiteur de type touriste
    /// </summary>
    public static void SpawnTourist()
    {
        instance.SpawnVisitor(instance.visitorTypes[0]);
    }

    /// <summary>
    /// Spawn un visiteur de type Hiker
    /// </summary>
    public static void SpawnHiker()
    {
        instance.SpawnVisitor(instance.visitorTypes[1]);
    }

    /// <summary>
    /// D�sactive un visiteur.
    /// </summary>
    /// <param name="toDelete">Le visiteur � d�sactiver.</param>
    public static void DeleteVisitor(VisitorBehavior toDelete)
    {
        isOnDespawn?.Invoke(true);
        toDelete.UnsetVisitor();

        instance.OnUpdateVisitorNumber?.Invoke(UsedVisitorNumber());

        OnDespawnVisitor?.Invoke(toDelete);
        OnChangeVisitorCount?.Invoke(UsedVisitorNumber());
    }

    /// <summary>
    /// Choisit al�atoirement un type de visiteur.
    /// </summary>
    /// <returns>Le type de visiteur choisit.</returns>
    private VisitorScriptable ChooseVisitorType()
    {
        return visitorTypes[UnityEngine.Random.Range(0, visitorTypes.Count)];
    }

    public static void AddVisitorType(VisitorScriptable nVisitorType)
    {
        if(!instance.visitorTypes.Contains(nVisitorType))
        {
            instance.visitorTypes.Add(nVisitorType);
        }
    }

    private IST_PathPoint SearchSpawnPoint(List<LandmarkType> landmarkTypes)
    {
        List<IST_PathPoint> possiblePathpoints = new List<IST_PathPoint>();
        List<IST_PathPoint> allSpawns = new List<IST_PathPoint>(PathManager.SpawnPoints);

        foreach (LandmarkType landmarkType in landmarkTypes)
        {
            for (int j = 0; j < allSpawns.Count; j++)
            {
                List<CPN_IsLandmark> landmarks = VLY_LandmarkManager.GetLandmarkOfType(landmarkType);

                for (int k = 0; k < landmarks.Count; k++)
                {
                    if (allSpawns[j].Node.HasValidPathForLandmark(landmarks[k]))
                    {
                        possiblePathpoints.Add(allSpawns[j]);
                    }
                }
            }
        }
        if (possiblePathpoints.Count > 0)
        {
            return possiblePathpoints[UnityEngine.Random.Range(0, possiblePathpoints.Count)];
        }
        else
        {
            return null;
        }
    }

    private IST_PathPoint SearchSpawnPointWithoutLandmark()
    {
        List<IST_PathPoint> possiblePathpoints = new List<IST_PathPoint>();
        List<IST_PathPoint> allSpawns = new List<IST_PathPoint>(PathManager.SpawnPoints);

        for (int j = 0; j < allSpawns.Count; j++)
        {
            List<CPN_IsLandmark> landmarks = VLY_LandmarkManager.GetLandmarkOfType(LandmarkType.Spawn);

            for (int k = 0; k < landmarks.Count; k++)
            {
                if (allSpawns[j].Node.GetDataForLandmarkType(landmarks[k]).linkedToLandmark == true && allSpawns[j].Node.HasNeighboursLinkedToSpawn())
                {
                    possiblePathpoints.Add(allSpawns[j]);
                }
            }
        }

        if (possiblePathpoints.Count > 0)
        {
            return possiblePathpoints[UnityEngine.Random.Range(0, possiblePathpoints.Count)];
        }
        else
        {
            return null;
        }
    }

    private CPN_IsLandmark SearchObjective(VisitorScriptable visitorType, PathNode spawnPoint)
    {
        List<CPN_IsLandmark> possibleObjectives = new List<CPN_IsLandmark>();

        for (int i = 0; i < visitorType.LandmarksWanted.Count; i++)
        {
            //Faire le tour de tous les IsLandmark de chaque type voulut par le visiteur
            List<CPN_IsLandmark> landmarks = VLY_LandmarkManager.GetLandmarkOfType(visitorType.LandmarksWanted[i]);

            for (int k = 0; k < landmarks.Count; k++)
            {
                if (spawnPoint.HasValidPathForLandmark(landmarks[k]))
                {
                    possibleObjectives.Add(landmarks[k]);
                }
            }
        }

        if (possibleObjectives.Count > 0)
        {
            return possibleObjectives[UnityEngine.Random.Range(0, possibleObjectives.Count)];
        }
        else
        {
            return null;
        }
    }

    private void SetType(VisitorBehavior visitorBehav)
    {
        CPN_Informations cpn_Inf = visitorBehav.GetComponent<CPN_Informations>();

        switch(visitorBehav.visitorType.name)
        {
            case "Hiker":
                cpn_Inf.visitorType = TypeVisitor.Hiker;
                break;
            case "Tourist":
                cpn_Inf.visitorType = TypeVisitor.Tourist;
                break;
        }
    }

    /// <summary>
    /// Cherche le premier VisitorBehavior qui n'est pas actif.
    /// </summary>
    /// <returns>Un VisitorBehavior inactif.</returns>
    private VisitorBehavior GetAvailableVisitor()
    {
        for (int i = 0; i < visitorPool.Count; i++)
        {
            if (!visitorPool[i].IsUsed)
            {
                return visitorPool[i];
            }
        }
        return null;
    }

    /// <summary>
    /// V�rifie le nombre de visiteur actif.
    /// </summary>
    /// <returns>Le nombre de vitieur actif.</returns>
    public static int UsedVisitorNumber()
    {
        int toReturn = 0;
        for (int i = 0; i < instance.visitorPool.Count; i++)
        {
            if (instance.visitorPool[i].IsUsed)
            {
                toReturn++;
            }
        }

        return toReturn;
    }

    public static List<VisitorBehavior> GetUsedVisitors()
    {
        return instance.UsedVisitorList();
    }

    private List<VisitorBehavior> UsedVisitorList()
    {
        List<VisitorBehavior> toReturn = new List<VisitorBehavior>();

        for (int i = 0; i < visitorPool.Count; i++)
        {
            if (visitorPool[i].IsUsed)
            {
                toReturn.Add(visitorPool[i]);
            }
        }

        return toReturn;
    }

    public static GameObject FindActiveHiker()
    {
        for (int i = 0; i < instance.visitorPool.Count; i++)
        {
            if (instance.visitorPool[i].IsUsed && instance.visitorPool[i].GetComponent<CPN_Informations>().visitorType == TypeVisitor.Hiker)
            {
                return instance.visitorPool[i].gameObject;
            }
        }

        return null;
    }

    public static List<VisitorBehavior> HikersList()
    {
        List<VisitorBehavior> toReturn = new List<VisitorBehavior>();

        for (int i = 0; i < instance.visitorPool.Count; i++)
        {
            if (instance.visitorPool[i].IsUsed && instance.visitorPool[i].GetComponent<CPN_Informations>().visitorType == TypeVisitor.Hiker)
            {
                toReturn.Add(instance.visitorPool[i]);
            }
        }

        return toReturn;
    }

    /// <summary>
    /// Choisit le chemin que devra emprunter un visiteur.
    /// </summary>
    /// <param name="visitorType">Le type de visiteur qui doit emprunter le chemin.</param>
    /// <param name="spawnPoint">Le point de spawn du chemin.</param>
    /// <returns>Le chemin que decra parcourir le visiteur.</returns>
    [System.Obsolete] public PathData ChoosePath(VisitorScriptable visitorType, IST_PathPoint spawnPoint)
    {
        List<PathData> allPath = PathManager.GetAllUsablePath(spawnPoint);
        List<PathData> possiblePath = new List<PathData>();

        for(int i = 0; i < allPath.Count; i++)
        {
            //Check des Priorit�s des Paths
            possiblePath.Add(allPath[i]);
        }

        if (possiblePath.Count > 0)
        {
            return possiblePath[UnityEngine.Random.Range(0, possiblePath.Count)];
        }
        else
        {
            return null;
        }
    }

    private void OnDestroy()
    {
        OnSpawnVisitor = null;
        OnDespawnVisitor = null;
        OnChangeVisitorCount = null;
}
}
