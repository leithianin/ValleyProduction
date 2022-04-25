using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : VLY_Singleton<PathManager>
{
    //List de data
    [SerializeField] private List<IST_PathPoint> spawnPoints;
    [SerializeField] private List<PathData> pathDataList = new List<PathData>();
    [SerializeField] private List<PathFragmentData> pathFragmentDataList = new List<PathFragmentData>();
                     public List<IST_PathPoint> pathpointList = new List<IST_PathPoint>();

    //Accesseur des listes
    public static List<IST_PathPoint> SpawnPoints => instance.spawnPoints;
    public static List<PathData> GetAllPath => instance.pathDataList;
    public static List<IST_PathPoint> GetCurrentPathpointList => instance.pathpointList;

    //Current Data
    private PathData currentPathData              = null;
    private PathData disconnectedPathData         = null;
    public static IST_PathPoint previousPathpoint = null;
    public static PathData pathDataToDelete = null;

    //Accesseur current Data
    public static PathData GetCurrentPathData => instance.currentPathData;

    //Uniquement pour delete un point
    private static List<IST_PathPoint> listAllPathPoints = new List<IST_PathPoint>();

    [SerializeField] private InterestPointDetector roadDetectorPrefab;

    [Header("DEBUG")]
    [SerializeField] private bool debugMode = false;
    [SerializeField] private GameObject DebugLineRenderer;
    [SerializeField] private List<GameObject> lineRendererDebugList = new List<GameObject>();

    private bool PathReverse       = false;
    public LineRenderer currentLineDebug;

    [Header("Action")]
    public static Action<bool> isOnSpawn;
    public static Action<bool> isOnFinishPath;

    #region Actions
    public static Action<PathData> OnCreatePath;
    public static Action<PathData> OnDestroyPath;
    #endregion

    public static PathManager GetInstance => instance;

    public static bool IsOnCreatePath => (GetCurrentPathpointList.Count>0)?true:false;

    public static PathData GetLastPathDataCreated => instance.pathDataList[instance.pathDataList.Count - 1];

    private void Start()
    {
        instance.currentPathData = null;
    }

    private void Update()
    {
        if (!debugMode || !ConstructionManager.HasSelectedStructureType || currentLineDebug == null)
        {
            Destroy(currentLineDebug);
        }
    }

    public static List<PathData> GetAllUsablePath(IST_PathPoint spawnPoint)
    {
        List<PathData> possiblePath = new List<PathData>();

        for(int i = 0; i < GetAllPath.Count; i++)
        {
            if(GetAllPath[i].ContainsPoint(spawnPoint))
            {
                possiblePath.Add(GetAllPath[i]);
            }
        }

        return new List<PathData>(possiblePath);
    }

    /// <summary>
    /// Get le path d'un pathpoint
    /// </summary>
    /// <param name="pathpoint"></param>
    /// <returns></returns>
    public static PathData GetPathData(IST_PathPoint pathpoint)
    {
        foreach(PathData pd in GetAllPath)
        {
            if(pd.ContainsPoint(pathpoint))
            {
                return pd;
            }
        }

        return null;
    }

    public void ResetCurrentData()
    {
        instance.pathFragmentDataList.Clear();
        instance.pathpointList.Clear();
        instance.currentPathData = null;
        previousPathpoint = null;

        PathCreationManager.ResetData();
    }

    /// <summary>
    /// Return if a Pathpoint is in several PathData
    /// </summary>
    /// <param name="pathpoint"></param>
    /// <returns></returns>
    public static bool HasManyPath(IST_PathPoint pathpoint)
    {
        int nb = 0;
        foreach (PathData pd in instance.pathDataList)
        {
            if (pd.ContainsPoint(pathpoint))
            {
                nb++;

                if (nb > 1)
                {
                    return true;
                }
            }
        }
        return false;
    }

    #region PathFragment
    /// <summary>
    /// Add the pathFragmentData to the list of pathFragment
    /// </summary>
    /// <param name="toAdd"></param>
    private void AddPathfragmentToList(PathFragmentData toAdd)
    {
        pathFragmentDataList.Add(toAdd);
        toAdd.startPoint.Node.AddFragment(toAdd);
        toAdd.endPoint.Node.AddFragment(toAdd);
    }

    private void RemoveLastPathFragmentToList()
    {
        PathFragmentData toRemove = pathFragmentDataList[pathFragmentDataList.Count - 1];

        PathRendererManager.DeletePathRenderer(toRemove);

        toRemove.DeleteFragmentData();

        pathFragmentDataList.RemoveAt(pathFragmentDataList.Count - 1);
    }

    /// <summary>
    /// Update Pathfragments data of a Path data
    /// </summary>
    public static void UpdatePathFragmentData(ModifiedPath toModify)
    {
        PathCreationManager.movingPathpoint.Node.ResetNodeData();

        PathData pd = toModify.pathData;

        while (pd.pathFragment.Count > 0)
        {
            pd.RemovePathFragment(pd.pathFragment[0]);
        }

        for (int i = 0; i < toModify.pathPoints.Count - 1; i++)
        {
            if (toModify.pathPoints[i] == null)
            {
                toModify.pathPoints.RemoveAt(i);
                i--;
            }
        }

        for (int i = 0; i < toModify.pathPoints.Count - 1; i++)
        {
            PathFragmentData new_pfd = new PathFragmentData(toModify.pathPoints[i], toModify.pathPoints[i + 1], PathCreationManager.instance.CalculatePath(toModify.pathPoints[i], toModify.pathPoints[i + 1]));
            pd.AddPathFragment(new_pfd);
        }

        if (instance.debugMode)
        {
            Debug.Log("Feedback visuel");
            DebugLineR(pd);
        }
    }

    #endregion

    #region Pathpoint
    //Create PathFragmentData
    public static void PlacePoint(IST_PathPoint pathpoint)
    {
        List<Vector3> navmeshPoints = new List<Vector3>(PathCreationManager.navmeshPositionsList);

        instance.pathpointList.Add(pathpoint);

        if(previousPathpoint != null)
        {
            //ChangementPathFragment
            PathFragmentData new_pfd = new PathFragmentData(previousPathpoint, pathpoint, navmeshPoints);
            instance.AddPathfragmentToList(new_pfd);

            PathCreationManager.instance.pathRendererManager.ManagePathRenderer(previousPathpoint, pathpoint, navmeshPoints, new_pfd);

            //IF ONBOARDING SEQUENCE 
            //new_pfd.CheckAvailableInterestPoint();

            previousPathpoint = pathpoint;
        }
        else
        {
            previousPathpoint = pathpoint;
        }

        pathpoint.Node.PlaceNode();

        DebugPoint(previousPathpoint);
    }

    public static void UnplacePoint(IST_PathPoint toRemove)
    {
        instance.pathpointList.Remove(toRemove);
        DestroyPreviousLine();

        if (instance.pathpointList.Count > 0)
        {
            previousPathpoint = instance.pathpointList[instance.pathpointList.Count - 1];

            instance.RemoveLastPathFragmentToList();
        }
        else
        {
            instance.ResetCurrentData();
        }
    }

    /// <summary>
    /// Delete the given pathpoint
    /// </summary>
    /// <param name="ist_pp"></param>
    /// <param name="pd"></param>
    public static void DeletePoint(IST_PathPoint ist_pp, PathData pd = null)
    {
        PathData pdToModify = new PathData();
        if (pd != null || pathDataToDelete != null)                         //Je ne peux arriver l� sans conna�tre le PathData � delete
        {
            if (pd == null) { pdToModify = pathDataToDelete; }
            else { pdToModify = pd; }
        }
        else
        {
            return;
        }

        if (SpawnPoints.Contains(ist_pp)) { SpawnPoints.Remove(ist_pp);}

        if (pdToModify != null)
        {
            switch (pdToModify.pathFragment.Count)
            {
                case 0:
                    DeletePointWith0PathFragment(pdToModify);
                    return;
                case 1:
                    DeletePointWith1PathFragment(pdToModify, ist_pp);
                    return;
                case 2:
                    DeletePointWith2PathFragment(pdToModify, ist_pp);
                    return;
                default :
                    DeletePointWith2MorePathFragment(pdToModify, ist_pp);
                    return;
            }
        }

        NodePathProcess.UpdateAllNodes();
    }

    #region 0 pathFragment
    /// <summary>
    /// If the path have just a pathpoint, delete the Path
    /// </summary>
    /// <param name="pdToModify"></param>
    public static void DeletePointWith0PathFragment(PathData pdToModify)
    {
        if (pdToModify.pathFragment.Count == 0)
        {
            DeletePath(pdToModify);
            return;
        }
    }
    #endregion

    #region 1 pathFragment
    /// <summary>
    /// If the path have 1 pathFragment
    /// </summary>
    /// <param name="pdToModify"></param>
    public static void DeletePointWith1PathFragment(PathData pdToModify, IST_PathPoint ist_pp)
    {
        //If the path have 1 PathFragment
        if (pdToModify.pathFragment.Count == 1)
        {
            if ((!HasManyPath(pdToModify.pathFragment[0].startPoint)) && (!HasManyPath(pdToModify.pathFragment[0].endPoint)))
            {
                DeletePath(pdToModify);
                pathDataToDelete = null;
                if (pdToModify.pathFragment[0].startPoint != ist_pp)
                {
                    pdToModify.pathFragment[0].startPoint.RemoveObject();
                }
                else
                {
                    pdToModify.pathFragment[0].endPoint.RemoveObject();
                }
            }
            else
            {
                DeletePath(pdToModify);
            }
            return;
        }
    }
    #endregion

    #region 2 pathFragment
    /// <summary>
    /// If the path have 2 pathFragment
    /// </summary>
    /// <param name="pdToModify"></param>
    public static void DeletePointWith2PathFragment(PathData pdToModify, IST_PathPoint ist_pp)
    {
        if (pdToModify.pathFragment.Count == 2)
        {
            //If is the middle point, delete the pathData
            if (pdToModify.pathFragment[1].HasThisStartingPoint(ist_pp) && pdToModify.pathFragment[0].HasThisEndingPoint(ist_pp))
            {
                //Need to check si un autre point hasManyPath
                if (HasManyPath(pdToModify.pathFragment[0].startPoint) || HasManyPath(pdToModify.pathFragment[1].endPoint))
                {
                    //Sans �a il reste un point seul, � Voir si �a casse pas des trucs 
                    if(HasManyPath(pdToModify.pathFragment[0].startPoint))
                    {
                        pdToModify.pathFragment[1].endPoint.RemoveObject();
                    }
                    else
                    {
                        pdToModify.pathFragment[0].startPoint.RemoveObject();
                    }

                    DeletePath(pdToModify);
                    return;
                }
                else
                {
                    DeletePath(pdToModify);
                    pdToModify.pathFragment[1].endPoint.RemoveObject();
                    pdToModify.pathFragment[0].startPoint.RemoveObject();
                    return;
                }
            }
            else
            {
                pdToModify.checkWichFragmentToRemove(ist_pp);
                DestroyLineRenderer(pdToModify.pathLineRenderer);
                DebugLineR(pdToModify);
                return;
            }
        }
    }

    #region >2 pathFragment
    /// <summary>
    /// If the path have more than 2 pathFragment
    /// </summary>
    /// <param name="pdToModify"></param>
    public static void DeletePointWith2MorePathFragment(PathData pdToModify, IST_PathPoint ist_pp)
    {
        //Si ce n'est pas le dernier point
        if (pdToModify.GetLastPoint() != ist_pp && pdToModify.pathFragment[pdToModify.pathFragment.Count - 1].startPoint != ist_pp)
        {
            RemoveMultiPath(pdToModify, ist_pp);
            //Check si ces deux points ont HasManyPath
            //Si non, comme d'hab
            //Si oui --> pdToModify.RemoveMultiPath();


            //pdToModify.RemoveMultiPath();
            List<PathFragmentData> pfdSecondPath = pdToModify.GetAllNextPathFragment(ist_pp);               //List of PathFragment after the deleted pathpoint

            pdToModify.RemoveFragmentAndNext(ist_pp);                                                       //Remove PathFragment where the pathpoint is + the next PathFragment 

            DestroyLineRenderer(pdToModify.pathLineRenderer);                                               //Destroy LineRenderer

            //Si StartPoint
            if (pdToModify.startPoint == ist_pp)
            {
                instance.pathDataList.Remove(pdToModify);
            }
            else
            {
                DebugLineR(pdToModify);
            }

            pdToModify.SafeCheck();                                                                         //Check if the path is Empty and delete it

            CreateCutPathData(pfdSecondPath);                                                             //Create a pathData with the second path
        }
        else
        {
            RemoveMultiPath(pdToModify, ist_pp);
            //pdToModify.RemoveMultiPath();
            pdToModify.RemoveFragmentAndNext(ist_pp);
            DestroyLineRenderer(pdToModify.pathLineRenderer);
            DebugLineR(pdToModify);
        }
    }

    public static void RemoveMultiPath(PathData pdToModify, IST_PathPoint ist_pp)
    {
        List<IST_PathPoint> pointsToCheck = new List<IST_PathPoint>(pdToModify.GetPointNextTo(ist_pp));

        foreach (IST_PathPoint pp in pointsToCheck)
        {
            if (HasManyPath(pp))
            {
                Debug.Log("tc");
            }
        }
    }

    #endregion

    /// <summary>
    /// Return if the point is a SpawnPoint
    /// </summary>
    /// <param name="pp"></param>
    /// <returns></returns>
    public static bool IsSpawnPoint(IST_PathPoint pp)
    {
        if (GetCurrentPathData != null)
        {
            if (GetCurrentPathData.startPoint == pp)
            {
                return true;
            }
        }

        if (instance.pathpointList.Count > 0 && instance.pathpointList[0] == pp)
        {
            return true;
        }

        return false;
    }
    #endregion

    #endregion

    #region PathData

    /// <summary>
    /// Delete the PathData with the pathPoint
    /// </summary>
    /// <param name="toDelete">PathData to delete</param>
    /// <param name="toIgnore">PathPoint to ignore</param>
    public static void DeleteFullPath(PathData toDelete, IST_PathPoint toIgnore = null)
    {
        List<IST_PathPoint> pointsToDelete = new List<IST_PathPoint>();
        pathDataToDelete = toDelete;

        for (int i = toDelete.pathFragment.Count - 1; i >= 0; i--)
        {
            if (!pointsToDelete.Contains(toDelete.pathFragment[i].endPoint) && toDelete.pathFragment[i].endPoint != toIgnore)
            {
                pointsToDelete.Add(toDelete.pathFragment[i].endPoint);
            }

            if (!pointsToDelete.Contains(toDelete.pathFragment[i].startPoint) && toDelete.pathFragment[i].startPoint != toIgnore)
            {
                pointsToDelete.Add(toDelete.pathFragment[i].startPoint);
            }
        }

        for (int j = 0; j < pointsToDelete.Count; j++)
        {
            if (IsPathDataStillExist(toDelete))
            {
                pointsToDelete[j].RemoveObject();
            }
        }

        OnDestroyPath?.Invoke(toDelete);
    }

    /// <summary>
    /// Delete the pathData but not the pathpoint
    /// </summary>
    /// <param name="pathdata"></param>
    public static void DeletePath(PathData pathdata)
    {
        DestroyLineRenderer(pathdata.pathLineRenderer);

        pathdata.DeletePathData();

        instance.pathDataList.Remove(pathdata);

        OnDestroyPath?.Invoke(pathdata);
    }

    /// <summary>
    /// Validate Path when right clic during a path is creating (Call in UnityEvent of InputPlayerManager)
    /// </summary>
    public void ValidatePath()
    {
        CreatePathData();
    }

    /// <summary>
    /// Create a new PathData
    /// </summary>
    public static void CreatePathData()
    {
        if (instance.pathpointList.Count > 1)
        {
            PathData newPathData = new PathData();

            newPathData.name = GeneratorManager.GetRandomPathName();                                                //Random Name
            newPathData.color = Color.grey;// new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), 1f);    //Random Color

            newPathData.pathFragment = new List<PathFragmentData>(instance.pathFragmentDataList);                   //List of created pathFragment

            newPathData.startPoint = instance.pathpointList[0];                                                     //Define the starting point of the path
            instance.pathDataList.Add(newPathData);

            //CODE REVIEW : Retirer les appellations debug vu que ce sera utilis� pour autre chose
            if (instance.debugMode)                                                                                 //LineRenderer
            {
                DebugLineR(newPathData);
            }

            NodePathProcess.UpdateAllNodes();
            newPathData.CheckMultiPath();

            instance.ResetCurrentData();                                                                            //Data to default

            OnCreatePath?.Invoke(newPathData);

            //IF ONBOARDING SEQUENCE 
            TimerManager.CreateRealTimer(0.5f, () => isOnFinishPath?.Invoke(true));
        }
        else if (instance.pathpointList.Count > 0)
        {
            instance.pathpointList[0].RemoveObject();
            instance.pathpointList.Clear();
        }
    }

    /// <summary>
    /// Create a PathData for the path that is cut by the deleting tool
    /// </summary>
    /// <param name="listPathFragment"></param>
    public static void CreateCutPathData(List<PathFragmentData> listPathFragment)
    {
        PathData newPathData = new PathData();

        newPathData.name = GeneratorManager.GetRandomPathName();                                                //Random Name
        newPathData.color = Color.grey; //new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), 1f);    //Random Color

        //CODE REVIEW : AddPathFragment pour les nodes � revoir par rapport � la fonction du dessus qui ne l'a pas
        newPathData.pathFragment = new List<PathFragmentData>();
        foreach (PathFragmentData pfd in listPathFragment)
        {
            newPathData.AddPathFragment(pfd);
        }

        newPathData.startPoint = listPathFragment[0].startPoint;
        instance.pathDataList.Add(newPathData);

        if (instance.debugMode)
        {
            DebugLineR(newPathData);
        }

        NodePathProcess.UpdateAllNodes();

        OnCreatePath?.Invoke(newPathData);
    }

    public static bool IsPathDataStillExist(PathData pathdata)
    {
        foreach(PathData pd in GetAllPath)
        {
            if(pd == pathdata)
            {
                return true;
            }
        }
        return false;
    }

    #endregion

    #region DEBUG
    /// <summary>
    /// Create a LineRenderer for the pathData
    /// </summary>
    /// <param name="pathData"></param>
    public static void DebugLineR(PathData pathData)
    {
        LineRenderer DEBUG = Instantiate(instance.DebugLineRenderer).GetComponent<LineRenderer>();
        DEBUG.material.color = pathData.color;

        int i = 0;

        DEBUG.positionCount = 0;

        foreach (PathFragmentData pfd in pathData.pathFragment)
        {
            foreach (Vector3 vector in pfd.path)                                                            //Set a position for each point in the path of the pathFragmentData
            {
                DEBUG.positionCount++;
                DEBUG.SetPosition(i, new Vector3(vector.x, vector.y + 0.25f, vector.z));
                i++;
            }

            List<InterestPointDetector> pathDetector = instance.GenerateDetectorsOnPath(pfd.path);          //PathDetector Logic
            for (int j = 0; j < pathDetector.Count; j++)
            {
                pathDetector[j].transform.SetParent(DEBUG.transform);
                pfd.AddInterestPointDetector(pathDetector[j]);
            }
        }

        pathData.pathLineRenderer = DEBUG;
        DestroyLineList();
    }

    /// <summary>
    /// Create a LineRenderer when you place a Point
    /// </summary>
    /// <param name="pathpoint"></param>
    /// <param name="color"></param>
    public static void DebugPoint(IST_PathPoint pathpoint, Color color = default(Color))
    {
        GameObject DEBUG = Instantiate(instance.DebugLineRenderer);
        instance.currentLineDebug = DEBUG.GetComponent<LineRenderer>();

        if(GetCurrentPathData != null)
        {
            instance.currentLineDebug.GetComponent<LineRenderer>().material.color = GetCurrentPathData.color;
        }

        instance.lineRendererDebugList.Add(DEBUG);

        instance.currentLineDebug.SetPosition(0, pathpoint.transform.position);

        instance.currentLineDebug.enabled = false;
    }

    /// <summary>
    /// Create a LineRenderer beetween 2 points (Use when you move a pathpoint)
    /// </summary>
    /// <param name="pp1"></param>
    /// <param name="pp2"></param>
    public static void DebugBetween2Points(IST_PathPoint pp1, IST_PathPoint pp2)
    {
        GameObject DEBUG = Instantiate(instance.DebugLineRenderer);

        if (GetCurrentPathData != null)
        {
            DEBUG.GetComponent<LineRenderer>().material.color = GetCurrentPathData.color;
        }

        instance.lineRendererDebugList.Add(DEBUG);

        DEBUG.GetComponent<LineRenderer>().SetPosition(0, pp1.transform.position);
        DEBUG.GetComponent<LineRenderer>().SetPosition(1, pp2.transform.position);
    }

    /// <summary>
    /// Destroy the list of LineRenderer because i created a single line for all the path
    /// </summary>
    public static void DestroyLineList()
    {
        foreach(GameObject go in instance.lineRendererDebugList)
        {
            Destroy(go);
        }

        instance.lineRendererDebugList.Clear();
    }

    /// <summary>
    /// Destroy a Line Renderer
    /// </summary>
    /// <param name="lineR"></param>
    public static void DestroyLineRenderer(LineRenderer lineR)
    {
        if (lineR != null)
        {
            Destroy(lineR.gameObject);
        }
    }

    /// <summary>
    /// Destroy the previous Line (Use when you validate a path, you don't need the last anymore)
    /// </summary>
    public static void DestroyPreviousLine()
    {
        instance.lineRendererDebugList.Remove(instance.currentLineDebug.gameObject);
        Destroy(instance.currentLineDebug.gameObject);

        if(instance.pathpointList.Count != 0)
        {
            instance.currentLineDebug = instance.lineRendererDebugList[instance.lineRendererDebugList.Count - 1].GetComponent<LineRenderer>();
        }
        else
        {
            instance.currentLineDebug = null;
        }
    }

    #region Moving
    /// <summary>
    /// Initialize all data that will be modified by the moving pathpoint
    /// </summary>
    /// <param name="pp"></param>
    public static void StartMovingPoint(IST_PathPoint pp)
    {
        foreach (PathData pd in instance.pathDataList)
        {
            if (pd.ContainsPoint(pp))
            {
                PathCreationManager.movingPathpoint = pp;

                Debug.Log(PathCreationManager.movingPathpoint);

                ModifiedPath nModifedPath= new ModifiedPath();

                nModifedPath.pathData = pd;
                nModifedPath.pathPoints = pd.GetAllPoints();
                nModifedPath.pathFragments = pd.pathFragment;

                instance.pathpointList.Clear();

                DestroyLineRenderer(pd.pathLineRenderer);

                PathCreationManager.isModifyPath = true;
                //Get Path Navmesh + Save Data

                PathCreationManager.ModifiedPaths.Add(nModifedPath);
            }
        }
    }

    /// <summary>
    /// Update linerenderer while moving
    /// </summary>
    /// <param name="pp"></param>
    public static void UpdateLineWhenMoving(IST_PathPoint pp)
    {
        foreach(ModifiedPath mp in PathCreationManager.ModifiedPaths)
        {
            for (int i = 0; i < mp.pathData.pathFragment.Count; i++)
            {
                mp.pathPoints.Add(mp.pathData.pathFragment[i].startPoint);

                if (i == mp.pathData.pathFragment.Count - 1)
                {
                    mp.pathPoints.Add(mp.pathData.pathFragment[i].endPoint);
                }

                DebugBetween2Points(mp.pathData.pathFragment[i].startPoint, mp.pathData.pathFragment[i].endPoint);

                if (mp.pathData.pathFragment[i].startPoint == pp || mp.pathData.pathFragment[i].endPoint == pp)
                {
                    mp.modifyList.Add(new ModifyListClass(instance.lineRendererDebugList[instance.lineRendererDebugList.Count - 1].GetComponent<LineRenderer>(), mp.pathData.pathFragment[i], mp.pathData.pathFragment[i].endPoint == pp));
                }
            }

            mp.pathPoints = mp.pathData.GetAllPoints();
            mp.pathFragments = mp.pathData.pathFragment;

            PathCreationManager.GetUpdateSeveralLine(mp);
            //Get Path Navmesh + Save Data
        }
    }

    /// <summary>
    /// Update the data with the new one when we finished to move
    /// </summary>
    /// <param name="pp"></param>
    public static void UpdateAfterMoving(IST_PathPoint pp)
    {
        PathCreationManager.isModifyPath = false;
        foreach (ModifiedPath mp in PathCreationManager.ModifiedPaths)
        {
            UpdatePathFragmentData(mp);
        }

        NodePathProcess.UpdateAllNodes();

        instance.ResetCurrentData();
    }
    #endregion
    #endregion

    #region PathDetector
    //CODE REVIEW : USEFULL ?
    public InterestPointDetector CreateDetector(Vector3 position, LineRenderer lineRenderer)
    {
        Vector3 target = position;
        InterestPointDetector detector = Instantiate(roadDetectorPrefab, lineRenderer.transform);
        detector.transform.position = target;

        return detector;
    }

    public float pointDistance = 2f;

    /// <summary>
    /// Generate Detectors Collider in the path
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private List<InterestPointDetector> GenerateDetectorsOnPath(List<Vector3> path)
    {
        if (path.Count <= 1)
        {
            return new List<InterestPointDetector>();
        }

        List<Vector3> detectorPositions = new List<Vector3>();

        float lineLength = 0;
        for (int i = 0; i < path.Count - 1; i++)
        {
            lineLength += Vector3.Distance(path[i], path[i + 1]);
        }

        detectorPositions.Add(path[0]);

        float distanceBetweenPoints = 2f;
        int numDist = (int)(lineLength / distanceBetweenPoints);
        int pathIndex = 0;

        for (int i = 0; i < numDist; i++)
        {
            float distanceLeft = distanceBetweenPoints;
            while (Vector3.Distance(detectorPositions[detectorPositions.Count - 1], path[pathIndex + 1]) <= distanceLeft)
            {
                distanceLeft -= Vector3.Distance(detectorPositions[detectorPositions.Count - 1], path[pathIndex + 1]);
                pathIndex++;
            }

            Vector3 nextPoint = detectorPositions[detectorPositions.Count - 1] + (path[pathIndex + 1] - path[pathIndex]).normalized * distanceLeft;

            detectorPositions.Add(nextPoint);
        }

        List<InterestPointDetector> toReturn = new List<InterestPointDetector>();

        for (int i = 0; i < detectorPositions.Count; i++)
        {
            InterestPointDetector detector = Instantiate(roadDetectorPrefab);
            detector.transform.position = detectorPositions[i];
            toReturn.Add(detector);
        }

        return toReturn;
    }

    //CODE REVIEW : USEFULL ?
    public bool CheckNextStepOnLine(Vector3 start, Vector3 next, ref float distance, ref float remainingDistance, out Vector3 point)
    {
        Vector3 direction = (next - start).normalized;
        float contextDistance = Vector3.Distance(start, next);
        distance += contextDistance;
        if (distance > pointDistance)
        {
            point = start + (direction * (pointDistance - remainingDistance));
            remainingDistance = distance = 0;
            return true;
        }
        point = default;
        remainingDistance = distance;
        return false;
    }
    #endregion 

    private void OnDestroy()
    {
        OnCreatePath = null;
        OnDestroyPath = null;
    }
}

