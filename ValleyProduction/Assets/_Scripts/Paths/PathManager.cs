using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : VLY_Singleton<PathManager>
{
    [SerializeField] private List<IST_PathPoint> spawnPoints;
    public static List<IST_PathPoint> SpawnPoints => instance.spawnPoints;

    //Get Data + Update Data
    private PathData currentPathData = null;
    private PathData disconnectedPathData = null;
    public List<PathData> pathDataList = new List<PathData>();
    public List<PathFragmentData> pathFragmentDataList = new List<PathFragmentData>();
    public List<IST_PathPoint> pathpointList = new List<IST_PathPoint>();
    public static IST_PathPoint previousPathpoint = null;

    //Uniquement pour delete un point
    private static List<IST_PathPoint> listAllPathPoints = new List<IST_PathPoint>();

    //Navmesh needs
    private static List<PathFragmentData> pfdNavmeshUpdate = new List<PathFragmentData>();

    [SerializeField] private InterestPointDetector roadDetectorPrefab;

    [Header("DEBUG")]
    public bool debugMode = false;
    public bool PathReverse = false;
    public static bool debugCheck = false;
    public GameObject DebugLineRenderer;
    public List<GameObject> lineRendererDebugList = new List<GameObject>();
    public LineRenderer currentLineDebug;

    [Header("Action")]
    public static System.Action<bool> isOnSpawn;
    public static System.Action<bool> isOnFinishPath;

    public static List<PathData> GetAllPath => instance.pathDataList;
    public static PathData GetCurrentPathData => instance.currentPathData;
    public static List<IST_PathPoint> GetCurrentPathpointList => instance.pathpointList;
    public static PathManager GetInstance => instance;

    public static bool IsOnCreatePath => (GetCurrentPathpointList.Count>0)?true:false;

    private void Start()
    {
        instance.currentPathData = null;
    }

    private void Update()
    {
        if (debugMode && ConstructionManager.HasSelectedStructureType && currentLineDebug != null)
        {
            //currentLineDebug.SetPosition(1, PlayerInputManager.GetMousePosition);
        }
        else
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

    private void AddPathfragmentToList(PathFragmentData toAdd)
    {
        pathFragmentDataList.Add(toAdd);

        toAdd.startPoint.Node.AddFragment(toAdd);
        toAdd.endPoint.Node.AddFragment(toAdd);
    }

    private void RemoveLastPathFragmentToList()
    {
        PathFragmentData toRemove = pathFragmentDataList[pathFragmentDataList.Count - 1];

        toRemove.DeleteFragmentData();

        pathFragmentDataList.RemoveAt(pathFragmentDataList.Count - 1);
    }

    //Create PathFragmentData
    public static void PlacePoint(IST_PathPoint pathpoint, Vector3 position)
    {
        List<Vector3> navmeshPoints = new List<Vector3>(PathCreationManager.navmeshPositionsList);

        instance.pathpointList.Add(pathpoint);

        if(previousPathpoint != null)
        {
            List<Vector3> allPoints = new List<Vector3>();
            allPoints.Add(previousPathpoint.transform.position);
            allPoints.Add(pathpoint.transform.position);

            //ChangementPathFragment
            PathFragmentData new_pfd = new PathFragmentData(previousPathpoint, pathpoint, navmeshPoints);
            instance.AddPathfragmentToList(new_pfd);

            PathCreationManager.instance.pathRendererManager.ManagePathRenderer(previousPathpoint, pathpoint, navmeshPoints);

            //IF ONBOARDING SEQUENCE 
            new_pfd.CheckAvailableInterestPoint();

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

    public static bool IsDeconnected(IST_PathPoint pathPoint)
    {
        foreach (PathData pd in instance.pathDataList)
        {
            if (pd.ContainsPoint(pathPoint) && pd.isDisconnected)
            {
                if (pathPoint == pd.startPoint)
                {
                    LinkDcPathToCurrentPath(pd, true);
                    return true;
                }
                else if(pathPoint == pd.GetLastPoint())
                {
                    LinkDcPathToCurrentPath(pd, false);
                    return true;
                }
            }
        }

        return false;
    }

    //Lié le current Path avec un chemin Déconnecté
    public static void LinkDcPathToCurrentPath(PathData pd , bool startPoint)
    {
        List<Vector3> navmeshPoints = new List<Vector3>(PathCreationManager.navmeshPositionsList);           //Doit prendre en compte le navmesh

        if (startPoint) //Logique quand le chemin est dans le bon sens
        {
            PathFragmentData new_pfd = new PathFragmentData(previousPathpoint, pd.startPoint, navmeshPoints);
            instance.AddPathfragmentToList(new_pfd);
        
            for (int i = 0; i < pd.pathFragment.Count; i++)
            {
                //Add les pathpoints
                instance.pathpointList.Add(pd.pathFragment[i].startPoint);

                DebugBetween2Points(previousPathpoint, pd.pathFragment[i].startPoint);
                previousPathpoint = pd.pathFragment[i].startPoint;

                if (i == pd.pathFragment.Count - 1)
                {
                    //Add le dernier pathpoint
                    instance.pathpointList.Add(pd.pathFragment[i].endPoint);

                    DebugBetween2Points(previousPathpoint, pd.pathFragment[i].endPoint);
                    previousPathpoint = pd.pathFragment[i].endPoint;
                }

                //Add les pathFragment
                instance.AddPathfragmentToList(pd.pathFragment[i]);
            }
        }
        else //Logique quand le chemin est inversé
        {
            PathFragmentData new_pfd = new PathFragmentData(previousPathpoint, pd.pathFragment[pd.pathFragment.Count - 1].endPoint, navmeshPoints);
            instance.AddPathfragmentToList(new_pfd);

            //Utiliser ça l'a haut directement
            for (int i = pd.pathFragment.Count-1; i >= 0; i--)
            {
                //Je remet dans le bon sens --> A mettre dans une fonction ?
                IST_PathPoint endPoint = pd.pathFragment[i].endPoint;
                pd.pathFragment[i].endPoint = pd.pathFragment[i].startPoint;
                pd.pathFragment[i].startPoint = endPoint;
                //Pareil pour le path--> Je prévois fonction car le path aura plus de 2 points dans le futur
                ChangePathDirection(pd.pathFragment[i]);

                //Add les pathpoints
                instance.pathpointList.Add(pd.pathFragment[i].startPoint);

                DebugBetween2Points(previousPathpoint, pd.pathFragment[i].startPoint);
                previousPathpoint = pd.pathFragment[i].startPoint;

                if (i == 0)
                {
                    //Add le dernier pathpoint
                    instance.pathpointList.Add(pd.pathFragment[i].endPoint);

                    DebugBetween2Points(previousPathpoint, pd.pathFragment[i].endPoint);
                    previousPathpoint = pd.pathFragment[i].endPoint;
                }

                //Add les pathFragment
                instance.AddPathfragmentToList(pd.pathFragment[i]);
                instance.PathReverse = true;
            }
        }

        instance.disconnectedPathData = pd;
        DebugPoint(previousPathpoint);
        //Destroy LineRenderer du pathData déconnecté
        DestroyLineRenderer(pd.pathLineRenderer);
    }

    public static void ChangePathDirection(PathFragmentData pfd)
    {
        Vector3 vec = pfd.path[0];
        pfd.path[0] = pfd.path[1];
        pfd.path[1] = vec;
    }

    //Obsolete
    /// <summary>
    /// Check if we can Destroy the pathpoint Gameobject. We check if is the last pathpoint and if he have more than 1 way.
    /// </summary>
    /// <param name="ist_pp"></param>
    /// <returns></returns>
    [System.Obsolete] public static bool CanDeleteGameobject(IST_PathPoint ist_pp)
    {
        listAllPathPoints.Clear();

        if (ist_pp != instance.pathpointList[instance.pathpointList.Count - 1])                                                  //Last pathpoint ?
        {
            return false;
        }

        int nb = 0;
        foreach (IST_PathPoint pathpoint in instance.pathpointList)                                                              //Other similar pathpoint?
        {
            if (pathpoint == ist_pp)
            {
                nb++;
            }

            if (nb > 1)                                                                                                          //If yes, delete the pathpoint but not the gameobject
            {
                DeletePoint(ist_pp);
                return false;
            }
        }

        foreach (PathData pd in instance.pathDataList)
        {
            for (int i = 0; i <= pd.pathFragment.Count - 1; i++)
            {
                if (i == pd.pathFragment.Count - 1)
                {
                    TriPathpointList(pd.pathFragment[i], listAllPathPoints, true);
                }
                else
                {
                    TriPathpointList(pd.pathFragment[i], listAllPathPoints);
                }
            }
        }

        nb = 0;
        foreach (IST_PathPoint pp in listAllPathPoints)
        {
            if(pp == ist_pp)
            {
                nb++;
            }

            if (nb > 1)                                                                                                          //If yes, delete the pathpoint but not the gameobject
            {
                DeletePoint(ist_pp);
                return false;
            }
        }

        DeletePoint(ist_pp);

        return true;
    }

    //Delete pathpoint
    public static void DeletePoint(IST_PathPoint ist_pp, PathData pd = null)
    {
        if (SpawnPoints.Contains(ist_pp))
        {
            SpawnPoints.Remove(ist_pp);
        }

        PathData pdToModify = new PathData();
        if (pd == null)
        {
            pdToModify = GetPathData(ist_pp);
        }
        else
        {
            pdToModify = pd;
        }

        if (pdToModify != null)
        {
            if(pdToModify.pathFragment.Count == 0)
            {
                DeletePath(pdToModify);
                return;
            }

            #region 1 pathFragment
            if (pdToModify.pathFragment.Count == 1)
            {
                DeletePath(pdToModify);
                if (pdToModify.pathFragment[0].startPoint != ist_pp)
                {
                    pdToModify.pathFragment[0].startPoint.RemoveObject();
                }
                else
                {
                    pdToModify.pathFragment[0].endPoint.RemoveObject();
                }
                return;
            }
            #endregion

            #region 2 pathFragment
            if (pdToModify.pathFragment.Count == 2)
            {
                if (pdToModify.pathFragment[1].HasThisStartingPoint(ist_pp) && pdToModify.pathFragment[0].HasThisEndingPoint(ist_pp))
                {
                    DeletePath(pdToModify);
                    pdToModify.pathFragment[1].endPoint.RemoveObject();
                    pdToModify.pathFragment[0].startPoint.RemoveObject();
                    return;
                }
                else
                {
                    pdToModify.checkWichFragmentToRemove(ist_pp);
                    DestroyLineRenderer(pdToModify.pathLineRenderer);
                    DebugLineR(pdToModify);
                    return;
                }
            }
            #endregion

            //Si ce n'est pas le dernier point
            if (pdToModify.GetLastPoint() != ist_pp && pdToModify.pathFragment[pdToModify.pathFragment.Count - 1].startPoint != ist_pp)
            {
                //Get les points après le pathpoint supprimé 
                List<PathFragmentData> pfdSecondPath = pdToModify.GetAllNextPathFragment(ist_pp);

                //Delete les PathFragment ou il y'a le pathpoint + Les pathFragment apres qu'on a save juste avant
                pdToModify.RemoveFragmentAndNext(ist_pp);

                //Créer un pathData avec le chemin fermé + Line renderer path fermé
                CreatePathDataClose(pfdSecondPath);

                //Delete line renderer 
                DestroyLineRenderer(pdToModify.pathLineRenderer);

                //Si StartPoint
                if (pdToModify.startPoint == ist_pp)
                {
                    instance.pathDataList.Remove(pdToModify);
                }
                else
                {
                    //Refaire LineRenderer pathData
                    //DebugLine pour avant
                    DebugLineR(pdToModify);
                }

                pdToModify.SafeCheck();
            }
            else
            {
                Debug.Log("LastPoint");
                pdToModify.RemoveFragmentAndNext(ist_pp);
                DestroyLineRenderer(pdToModify.pathLineRenderer);
                DebugLineR(pdToModify);
            }
        }

        NodePathProcess.UpdateAllNodes();
    }

    public static void DeleteFullPath(PathData toDelete)
    {
        DeleteFullPathWithoutOnePoint(toDelete, null);
    }

    public static void DeleteFullPathWithoutOnePoint(PathData toDelete, IST_PathPoint toIgnore)
    {
        
        List<IST_PathPoint> pointsToDelete = new List<IST_PathPoint>();

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
            pointsToDelete[j].RemoveObject();
        }
    }


    public static void DeletePath(PathData pathdata)
    {
        DestroyLineRenderer(pathdata.pathLineRenderer);

        pathdata.DeletePathData();

        instance.pathDataList.Remove(pathdata);
    }


    public void ValidatePath(GameObject hitedGameobject)
    {
        if(!hitedGameobject.TryGetComponent<IST_PathPoint>(out IST_PathPoint hitedPoint))
        {
            CreatePathData();
        }
    }

    //Create pathdata
    public static void CreatePathData()
    {
        if (instance.pathpointList.Count > 1)
        {
            //Obsolete pour le moment --> Pas de modifier un chemin existant
            if (instance.currentPathData != null && instance.currentPathData.name != string.Empty)
            {
                Debug.Log("String pas empty");
                //Mise à jour du PathData existant
                foreach (PathFragmentData pfd in instance.currentPathData.pathFragment)
                {
                    if (!instance.currentPathData.pathFragment.Contains(pfd))
                    {
                        //pfd.CheckAvailableInterestPoint();
                        instance.currentPathData.pathFragment.Add(pfd);
                    }
                }

                if (instance.debugMode)
                {
                    Debug.Log("Feedback visuel");
                    DebugLineR(instance.currentPathData);
                }
            }
            else
            {
                //Création PathData
                PathData newPathData = new PathData();

                //Random du chemin pour varier les infos
                newPathData.name = GeneratorManager.GetRandomPathName();
                //newPathData.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
                newPathData.color = new Color(Random.Range(0f, 1f),Random.Range(0f, 1f),Random.Range(0f, 1f),1f);

                //Remplissage des infos qu'on a 
                newPathData.pathFragment = new List<PathFragmentData>(instance.pathFragmentDataList);
                /*foreach(PathFragmentData pfd in newPathData.pathFragment)
                {
                    pfd.CheckAvailableInterestPoint();
                }*/

                newPathData.startPoint = instance.pathpointList[0];
                instance.pathDataList.Add(newPathData);

                if (instance.debugMode)
                {
                    DebugLineR(newPathData);
                }

                NodePathProcess.UpdateAllNodes();
            }

            if(instance.disconnectedPathData != null)
            {
                Destroy(instance.disconnectedPathData.pathLineRenderer);
                instance.pathDataList.Remove(instance.disconnectedPathData);
            }

            //Reset les currents Data puisqu'on deselectionne le chemin
            instance.ResetCurrentData();

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
    /// Update Pathfragments data of a Path data
    /// </summary>
    public static void UpdatePathFragmentData()
    {
        PathCreationManager.MovingPathPoint.Node.ResetNodeData();

        while (GetCurrentPathData.pathFragment.Count > 0)
        {
            GetCurrentPathData.RemovePathFragment(GetCurrentPathData.pathFragment[0]);
        }

        for(int i = 0; i < GetCurrentPathpointList.Count-1; i++)
        {
            PathFragmentData new_pfd = new PathFragmentData(GetCurrentPathpointList[i], GetCurrentPathpointList[i+1], PathCreationManager.instance.CalculatePath(GetCurrentPathpointList[i], GetCurrentPathpointList[i + 1]));
            GetCurrentPathData.AddPathFragment(new_pfd);
        }


        /*List<PathNode> pointsToCheck = PathCreationManager.GetMovingBorderPoints;

        for (int i = 0; i < pointsToCheck.Count; i++)
        {
            pointsToCheck[i].ResetNodeData();
        }*/

        NodePathProcess.UpdateAllNodes();

        /*for(int i = 0; i < PathCreationManager.GetAdditionnalPoints.Count; i++)
        {
            List<IST_PathPoint> pathPointAdditionnals = PathCreationManager.GetAdditionnalPoints[i].pathpointList;

            for(int j = 0; j < pathPointAdditionnals.Count; j++)
            {
                pathPointAdditionnals[j].Node.ResetNodeAndNeighbours();
            }
        }*/

        /*List<PathNode> pointsToCheck = PathCreationManager.GetMovingBorderPoints;

        instance.UpdateModifiedPathNodeDatas(pointsToCheck);*/

        //PathCreationManager.MovingPathPoint.Node.UpdateNode();

        //CODE REVIEW Voir pour corriger l'Update des nodes. Voir pour placer les nouveaux points
        //GetCurrentPathData.pathFragment[0].startPoint.Node.UpdateNode();

        if (instance.debugMode)
        {
            Debug.Log("Feedback visuel");
            DebugLineR(instance.currentPathData);
        }

        instance.ResetCurrentData();
    }

    private void UpdateModifiedPathNodeDatas(List<PathNode> nodesToUpdates)
    {
        if(nodesToUpdates.Count > 0)
        {
            nodesToUpdates[0].UpdateNode();
            nodesToUpdates.RemoveAt(0);
            TimerManager.CreateRealTimer(Time.deltaTime, () => UpdateModifiedPathNodeDatas(new List<PathNode>(nodesToUpdates)));
        }
    }

    public void ResetCurrentData()
    {
        instance.pathFragmentDataList.Clear();
        instance.pathpointList.Clear();
        instance.currentPathData = null;
        previousPathpoint = null;

        PathCreationManager.ResetData();
    }

    public static void CreatePathDataClose(List<PathFragmentData> listPathFragment)
    {
        PathData newPathData = new PathData();

        newPathData.name = string.Empty;
        newPathData.color = Color.gray; //Gris à choisir
        newPathData.pathFragment = new List<PathFragmentData>(listPathFragment);
        newPathData.startPoint = listPathFragment[0].startPoint;
        instance.pathDataList.Add(newPathData);
        newPathData.isDisconnected = true;

        if (instance.debugMode)
        {
            DebugLineR(newPathData);
        }
    }

    /// <summary>
    /// Selectionner un chemin à partir d'un pathpoint.
    /// </summary>
    /// <param name="pathpoint"></param>
    public static void SelectPath(IST_PathPoint pathpoint)
    {
        //Si la liste est vide c'est qu'aucun chemin n'est selectionné
        if (instance.pathpointList.Count == 0)
        {
            //CreatePathData();                                                           //Si il selectionne un autre chemin, on save celui en cours quand même

            foreach (PathData pd in instance.pathDataList)
            {
                if (pd.ContainsPoint(pathpoint))
                {
                    instance.currentPathData = pd;
                    UIManager.ShowRoadsInfos(pd);

                    //Il faut trouver les pathpoints
                    for (int i = 0; i <= pd.pathFragment.Count - 1; i++)
                    {
                        instance.AddPathfragmentToList(pd.pathFragment[i]);

                        if (i == pd.pathFragment.Count - 1)
                        {
                            TriPathpointList(pd.pathFragment[i], instance.pathpointList, true);
                        }
                        else
                        {
                            TriPathpointList(pd.pathFragment[i], instance.pathpointList);
                        }
                    }
                }
            }

            if (instance.pathpointList.Count != 0)
            {
                previousPathpoint = instance.pathpointList[instance.pathpointList.Count - 1];
            }

            DestroyLinePath(instance.currentPathData);
            DebugPoint(previousPathpoint, instance.currentPathData.color);
        }
    }

    //Check si le pathpoint est dans le current PathData
    public static bool IsOnCurrentPathData(IST_PathPoint pathpoint)
    {
        if (instance.currentPathData != null)
        {
            if (instance.currentPathData.ContainsPoint(pathpoint))
            {
                return true;
            }
        }

        return false;
    }

    //Selectionner un chemin avec les bouttons UI
    public static void SelectPathWithPathData(PathData pathdata)
    {
        CreatePathData();

        instance.currentPathData = pathdata;
        UIManager.ShowRoadsInfos(pathdata);

        for (int i = 0; i <= pathdata.pathFragment.Count - 1; i++)
        {
            instance.AddPathfragmentToList(pathdata.pathFragment[i]);

            if (i == pathdata.pathFragment.Count - 1)
            {
                TriPathpointList(pathdata.pathFragment[i], instance.pathpointList, true);
            }
            else
            {
                TriPathpointList(pathdata.pathFragment[i], instance.pathpointList);
            }
        }

        if (instance.pathpointList.Count != 0)
        {
            previousPathpoint = instance.pathpointList[instance.pathpointList.Count - 1];
        }

        DestroyLinePath(instance.currentPathData);
        DebugPoint(previousPathpoint, instance.currentPathData.color);
    }

    //Retourne si il y'a plusieurs chemin pour un pathpoint
    public static bool HasManyPath(IST_PathPoint pathpoint)
    {
        int nb = 0;
        foreach(PathData pd in instance.pathDataList)
        {
            if(pd.ContainsPoint(pathpoint))
            {
                nb++;

                if(nb > 1)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static bool HasOnePath(IST_PathPoint pathpoint)
    {
        foreach (PathData pd in instance.pathDataList)
        {
            if (pd.ContainsPoint(pathpoint))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Juste un tri des points pour ne pas mettre les mêmes.
    /// </summary>
    /// <param name="pfd"></param>
    public static void TriPathpointList(PathFragmentData pfd, List<IST_PathPoint> pathpointList, bool isLast = false)
    {
        pathpointList.Add(pfd.startPoint);

        if(isLast)
        {
            pathpointList.Add(pfd.endPoint);
        }
    }

    public static bool IsPathpointOnCurrentList(IST_PathPoint pathpoint)
    {
        if(instance.pathpointList.Contains(pathpoint))
        {
            return true;
        }

        return false;
    }

    public static bool IsPathpointListEmpty()
    {
        if(instance.pathpointList.Count > 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

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

    #region DEBUG
    //Linerenderer de tous le chemin
    public static void DebugLineR(PathData pathData)
    {
        LineRenderer DEBUG = Instantiate(instance.DebugLineRenderer).GetComponent<LineRenderer>();
        DEBUG.material.color = pathData.color;

        int i = 0;
        //DEBUG.GetComponent<LineRenderer>().positionCount = pathData.pathFragment.Count * 2;
        DEBUG.positionCount = 0;
        if (instance.PathReverse)
        {
            //Pas réussi avec le path
            for (int j = 0; j < instance.pathpointList.Count; j++)
            {
                DEBUG.positionCount++;
                DEBUG.SetPosition(j, instance.pathpointList[j].transform.position);
            }

            instance.PathReverse = false;
        }
        else
        {
            foreach (PathFragmentData pfd in pathData.pathFragment)
            {
                foreach (Vector3 vector in pfd.path)
                {
                    DEBUG.positionCount++;
                    DEBUG.SetPosition(i, vector);
                    i++;
                }
                List<InterestPointDetector> pathDetector = instance.GenerateDetectorsOnPath(pfd.path);

                for(int j = 0; j < pathDetector.Count; j++)
                {
                    pathDetector[j].transform.SetParent(DEBUG.transform);
                    pfd.AddInterestPointDetector(pathDetector[j]);
                }
                
                // Ajouter les liens vers le PathFragmentData et les Update des détecteurs
            }
        }

        //Je garde le chemin en mémoire 
        pathData.pathLineRenderer = DEBUG;

        DestroyLineList();
    }

    public float pointDistance = 2f;

    public InterestPointDetector CreateDetector(Vector3 position, LineRenderer lineRenderer)
    {
        Vector3 target = position;
        InterestPointDetector detector = Instantiate(roadDetectorPrefab, lineRenderer.transform);
        detector.transform.position = target;

        return detector;
        
    }

    private List<InterestPointDetector> GenerateDetectorsOnPath(List<Vector3> path)
    {
        if (path.Count <= 1)
        {
            return new List<InterestPointDetector>();
        }

        List<Vector3> detectorPositions = new List<Vector3>();

        float lineLength = 0;
        for(int i = 0; i < path.Count-1; i++)
        {
            lineLength += Vector3.Distance(path[i], path[i + 1]);
        }

        detectorPositions.Add(path[0]);

        float distanceBetweenPoints = 2f;
        int numDist = (int)(lineLength / distanceBetweenPoints);
        int pathIndex = 0;

        for(int i = 0; i < numDist; i++)
        {
            float distanceLeft = distanceBetweenPoints;
            while (Vector3.Distance(detectorPositions[detectorPositions.Count -1], path[pathIndex + 1]) <= distanceLeft)
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

    //Destroy le lineRenderer du pathData et crée les line renderer pour chaque PathFragment
    public static void DestroyLinePath(PathData pathData)
    {
        Destroy(pathData.pathLineRenderer.gameObject);
        pathData.pathLineRenderer = null;

        //Create Line Renderer
        foreach(PathFragmentData pfd in pathData.pathFragment)
        {
            GameObject DEBUG = Instantiate(instance.DebugLineRenderer);
            DEBUG.GetComponent<LineRenderer>().SetPosition(0, pfd.startPoint.transform.position);
            DEBUG.GetComponent<LineRenderer>().SetPosition(1, pfd.endPoint.transform.position);

            DEBUG.GetComponent<LineRenderer>().material.color = pathData.color;

            instance.lineRendererDebugList.Add(DEBUG);
        }
    }

    //LineRenderer entre chaque point
    public static void DebugPoint(IST_PathPoint pathpoint, Color color = default(Color))
    {
        GameObject DEBUG = Instantiate(instance.DebugLineRenderer);
        instance.currentLineDebug = DEBUG.GetComponent<LineRenderer>();

        if(instance.currentPathData != null)
        {
            instance.currentLineDebug.GetComponent<LineRenderer>().material.color = instance.currentPathData.color;
        }

        instance.lineRendererDebugList.Add(DEBUG);

        instance.currentLineDebug.SetPosition(0, pathpoint.transform.position);

        instance.currentLineDebug.enabled = false;
    }

    
    public static void DebugBetween2Points(IST_PathPoint pp1, IST_PathPoint pp2)
    {
        GameObject DEBUG = Instantiate(instance.DebugLineRenderer);
        //instance.currentLineDebug = DEBUG.GetComponent<LineRenderer>();

        if (instance.currentPathData != null)
        {
            //instance.currentLineDebug.material.color = instance.currentPathData.color;
            DEBUG.GetComponent<LineRenderer>().material.color = instance.currentPathData.color;
        }

        instance.lineRendererDebugList.Add(DEBUG);
        //instance.currentLineDebug.SetPosition(0, pp1.transform.position);
        //instance.currentLineDebug.SetPosition(1, pp2.transform.position);
        DEBUG.GetComponent<LineRenderer>().SetPosition(0, pp1.transform.position);
        DEBUG.GetComponent<LineRenderer>().SetPosition(1, pp2.transform.position);
    }

    //Destroy la list de lineRenderer (Puisque je fais un line renderer pour tout le path)
    public static void DestroyLineList()
    {
        foreach(GameObject go in instance.lineRendererDebugList)
        {
            Destroy(go);
        }

        instance.lineRendererDebugList.Clear();
    }

    public static void DestroyLineRenderer(LineRenderer lineR)
    {
        Destroy(lineR.gameObject);
    }

    //Destroy le dernier LineRenderer 
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

    //Destroy le line renderer du pathData pour mettre un line sur chaque pathpoint (Comme à la création)
    public static void UpdateLineWhenMoving(IST_PathPoint pp)
    {
        //DestroyLine du path ou il y'a le Pathpoint
        foreach(PathData pd in instance.pathDataList)
        {
            if(pd.ContainsPoint(pp))
            {
                DestroyLineRenderer(pd.pathLineRenderer);
                instance.currentPathData = pd;
            }

            for(int i = 0; i < pd.pathFragment.Count; i++)
            {
                instance.pathpointList.Add(pd.pathFragment[i].startPoint);

                if (i == pd.pathFragment.Count-1)
                {
                    instance.pathpointList.Add(pd.pathFragment[i].endPoint);
                }
                
                DebugBetween2Points(pd.pathFragment[i].startPoint, pd.pathFragment[i].endPoint);

                if (pd.pathFragment[i].startPoint == pp || pd.pathFragment[i].endPoint == pp)
                {
                    if (pd.pathFragment[i].endPoint == pp)
                    {
                        PathCreationManager.ModifyList.Add(new PathCreationManager.ModifyListClass(instance.lineRendererDebugList[instance.lineRendererDebugList.Count - 1].GetComponent<LineRenderer>(), pd.pathFragment[i],true));
                    }
                    else
                    {
                        PathCreationManager.ModifyList.Add(new PathCreationManager.ModifyListClass(instance.lineRendererDebugList[instance.lineRendererDebugList.Count - 1].GetComponent<LineRenderer>(), pd.pathFragment[i], false));
                    }
                }
            }
        }
        PathCreationManager.refList = GetCurrentPathpointList;
        PathCreationManager.instance.newPathFragmentData = new List<PathFragmentData>(GetCurrentPathData.pathFragment);
        PathCreationManager.GetUpdateSeveralLine(pp);
        PathCreationManager.isModifyPath = true;
        //Get Path Navmesh + Save Data
    }

    public static void UpdateAfterMoving(IST_PathPoint pp)
    {
        PathCreationManager.isModifyPath = false;
        PathCreationManager.ModifyList.Clear();
        UpdatePathFragmentData();
    }
    #endregion
}
