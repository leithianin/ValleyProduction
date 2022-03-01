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

    [SerializeField] private VisitorScriptable[] visitorTypes;

    [SerializeField] private List<VisitorBehavior> visitorPool;

    [SerializeField] private Terrain mainTerrain;

    public static System.Action<bool> isOnDespawn;

    private float nextSpawnTime = 5f;

     [SerializeField] private bool allowVisitorSpawn;

    public static Terrain GetMainTerrain => instance.mainTerrain;

    private void Update()
    {
        if (Time.time > nextSpawnTime && allowVisitorSpawn)
        {
            int spawnNb = Random.Range(visitorToSpawnNb.x, visitorToSpawnNb.y + 1);
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
    /// Demande à faire appraître un visiteur.
    /// </summary>
    private void SpawnVisitor(VisitorScriptable type = null)
    {
        if (PathManager.SpawnPoints.Count > 0)
        {
            VisitorBehavior newVisitor = GetAvailableVisitor();

            Vector2 rng = UnityEngine.Random.insideUnitCircle * spawnDistanceFromSpawnPoint;
            IST_PathPoint wantedSpawn = PathManager.SpawnPoints[Random.Range(0, PathManager.SpawnPoints.Count)];
            Vector3 spawnPosition = wantedSpawn.transform.position + new Vector3(rng.x, 0, rng.y);

            NavMeshHit hit;

            if (newVisitor != null && NavMesh.SamplePosition(spawnPosition, out hit, 5f, NavMesh.AllAreas))
            {
                VisitorScriptable visitorType;
                if (type != null) { visitorType = type; }
                else { visitorType = ChooseVisitorType(); }
   
                PathData chosenPath = ChoosePath(visitorType, wantedSpawn);

                if (chosenPath != null)
                {
                    newVisitor.SetVisitor(wantedSpawn, spawnPosition, visitorType, chosenPath);
                    SetType(newVisitor);             
                }
                else
                {
                    //Debug.LogError("Error : ChosenPath est null.");
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
    /// Désactive un visiteur.
    /// </summary>
    /// <param name="toDelete">Le visiteur à désactiver.</param>
    public static void DeleteVisitor(VisitorBehavior toDelete)
    {
        isOnDespawn?.Invoke(true);
        toDelete.UnsetVisitor();
    }

    /// <summary>
    /// Choisit aléatoirement un type de visiteur.
    /// </summary>
    /// <returns>Le type de visiteur choisit.</returns>
    private VisitorScriptable ChooseVisitorType()
    {
        return visitorTypes[Random.Range(0, visitorTypes.Length)];
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
            if (!visitorPool[i].gameObject.activeSelf)
            {
                return visitorPool[i];
            }
        }
        return null;
    }

    /// <summary>
    /// Vérifie le nombre de visiteur actif.
    /// </summary>
    /// <returns>Le nombre de vitieur actif.</returns>
    public static int UsedVisitorNumber()
    {
        int toReturn = 0;
        for (int i = 0; i < instance.visitorPool.Count; i++)
        {
            if (instance.visitorPool[i].gameObject.activeSelf)
            {
                toReturn++;
            }
        }

        return toReturn;
    }

    public static GameObject FindActiveHiker()
    {
        for (int i = 0; i < instance.visitorPool.Count; i++)
        {
            if (instance.visitorPool[i].gameObject.activeSelf && instance.visitorPool[i].GetComponent<CPN_Informations>().visitorType == TypeVisitor.Hiker)
            {
                return instance.visitorPool[i].gameObject;
            }
        }

        return null;
    }

    /// <summary>
    /// Choisit le chemin que devra emprunter un visiteur.
    /// </summary>
    /// <param name="visitorType">Le type de visiteur qui doit emprunter le chemin.</param>
    /// <param name="spawnPoint">Le point de spawn du chemin.</param>
    /// <returns>Le chemin que decra parcourir le visiteur.</returns>
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
