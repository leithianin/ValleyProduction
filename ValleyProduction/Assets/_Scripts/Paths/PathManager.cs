using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : VLY_Singleton<PathManager>
{
    //Get Data + Update Data
    private PathData currentPathData = null; 
    public List<PathData> pathDataList = new List<PathData>();
    public List<PathFragmentData> pathFragmentDataList = new List<PathFragmentData>();
    public List<IST_PathPoint> pathpointList = new List<IST_PathPoint>();
    public static IST_PathPoint previousPathpoint = null;

    //Uniquement pour delete un point
    private static List<IST_PathPoint> listAllPathPoints = new List<IST_PathPoint>();

    [Header("DEBUG")]
    public bool debugMode = false;
    public static bool debugCheck = false;
    public GameObject DebugLineRenderer;
    public List<GameObject> lineRendererDebugList = new List<GameObject>();
    public LineRenderer currentLineDebug;

    public static List<PathData> GetAllPath => instance.pathDataList;
    public static PathData GetCurrentPathData => instance.currentPathData;

    private void Update()
    {
        if (debugMode && ConstructionManager.HasSelectedStructureType && currentLineDebug != null)
        {
            currentLineDebug.SetPosition(1, PlayerInputManager.GetMousePosition);
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

    //Create PathFragmentData
    public static void PlacePoint(IST_PathPoint pathpoint, Vector3 position)
    {
        instance.pathpointList.Add(pathpoint);

        if(previousPathpoint != null)
        {
            //List<Vector3> allPoints = new List<Vector3>(PathCreationManager.navmeshPositionsList);
            List<Vector3> allPoints = new List<Vector3>();
            allPoints.Add(previousPathpoint.transform.position);
            allPoints.Add(pathpoint.transform.position);

            instance.pathFragmentDataList.Add(new PathFragmentData(previousPathpoint, pathpoint, allPoints));
            previousPathpoint = pathpoint;
        }
        else
        {
            previousPathpoint = pathpoint;
        }

        DebugPoint(previousPathpoint);
    }

    /// <summary>
    /// Check if we can Destroy the pathpoint Gameobject. We check if is the last pathpoint and if he have more than 1 way.
    /// </summary>
    /// <param name="ist_pp"></param>
    /// <returns></returns>
    public static bool CanDeleteGameobject(IST_PathPoint ist_pp)
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
    public static void DeletePoint(IST_PathPoint ist_pp)
    {
        //Remove PathFragmentData
        if (instance.pathFragmentDataList.Count != 0)
        {
            if (instance.currentPathData != null)
            {
                instance.currentPathData.RemoveFragment(instance.pathFragmentDataList[instance.pathFragmentDataList.Count - 1]);
            }

            instance.pathFragmentDataList.RemoveAt(instance.pathFragmentDataList.Count - 1);
        }
        else
        {
            //Remove PathData (Puisqu'il est vide)         
            instance.pathDataList.Remove(instance.currentPathData);
            instance.currentPathData = null;
        }

        //Remove Pathpoint
        Debug.Log(instance.pathpointList.Count);
        instance.pathpointList.RemoveAt(instance.pathpointList.Count-1);

        //Change le Previous Pathpoint
        if (instance.pathpointList.Count != 0)
        {
            previousPathpoint = instance.pathpointList[instance.pathpointList.Count - 1];
        }
        else
        {
            UIManager.HideRoadsInfo();
        }

        //Remove LineRenderer
        if(instance.debugMode && instance.lineRendererDebugList.Count != 0)
        {
            DestroyPreviousLine();
        }
    }

    //Create pathdata
    public static void CreatePathData()
    {
        if (instance.pathpointList.Count > 0)
        {
            if (instance.currentPathData != null)
            {
                //Mise � jour du PathData existant
                foreach (PathFragmentData pfd in instance.pathFragmentDataList)
                {
                    if (!instance.currentPathData.pathFragment.Contains(pfd))
                    {
                        pfd.CheckAvailableInterestPoint();
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
                //Cr�ation PathData
                PathData newPathData = new PathData();

                //Random du chemin pour varier les infos
                newPathData.name = RandomPathName.GetRandomName();
                newPathData.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

                //Remplissage des infos qu'on a 
                newPathData.pathFragment = new List<PathFragmentData>(instance.pathFragmentDataList);
                foreach(PathFragmentData pfd in newPathData.pathFragment)
                {
                    pfd.CheckAvailableInterestPoint();
                }

                newPathData.startPoint = instance.pathpointList[0];
                instance.pathDataList.Add(newPathData);

                if (instance.debugMode)
                {
                    Debug.Log("Feedback visuel");
                    DebugLineR(newPathData);
                }
            }

            //Reset les currents Data puisqu'on deselectionne le chemin
            instance.pathFragmentDataList.Clear();
            instance.pathpointList.Clear();
            instance.currentPathData = null;
            previousPathpoint = null;
        }
    }

    /// <summary>
    /// Selectionner un chemin � partir d'un pathpoint.
    /// </summary>
    /// <param name="pathpoint"></param>
    public static void SelectPath(IST_PathPoint pathpoint)
    {
        //Si la liste est vide c'est qu'aucun chemin n'est selectionn�
        if (instance.pathpointList.Count == 0)
        {
            //CreatePathData();                                                           //Si il selectionne un autre chemin, on save celui en cours quand m�me

            foreach (PathData pd in instance.pathDataList)
            {
                if (pd.ContainsPoint(pathpoint))
                {
                    instance.currentPathData = pd;
                    UIManager.ShowRoadsInfos(pd);

                    //Il faut trouver les pathpoints
                    for (int i = 0; i <= pd.pathFragment.Count - 1; i++)
                    {
                        instance.pathFragmentDataList.Add(pd.pathFragment[i]);

                        if (i == pd.pathFragment.Count - 1)
                        {
                            TriPathpointList(pd.pathFragment[i], instance.pathpointList, true);
                        }
                        else
                        {
                            TriPathpointList(pd.pathFragment[i], instance.pathpointList);
                        }
                    }

                    /*foreach (PathFragmentData pfd in pd.pathFragment)
                    {
                        instance.pathFragmentDataList.Add(pfd);
                        TriPathpointList(pfd, instance.pathpointList);
                    }*/
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
            instance.pathFragmentDataList.Add(pathdata.pathFragment[i]);

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

    /// <summary>
    /// Juste un tri des points pour ne pas mettre les m�mes.
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

    #region DEBUG
    //Linerenderer de tous le chemin
    public static void DebugLineR(PathData pathData)
    {
        GameObject DEBUG = Instantiate(instance.DebugLineRenderer);
        DEBUG.GetComponent<LineRenderer>().material.color = pathData.color;

        int i = 0;
        DEBUG.GetComponent<LineRenderer>().positionCount = pathData.pathFragment.Count * 2;
        foreach (PathFragmentData pfd in pathData.pathFragment)
        {
            DEBUG.GetComponent<LineRenderer>().SetPosition(i, pfd.startPoint.transform.position);
            i++;
            DEBUG.GetComponent<LineRenderer>().SetPosition(i, pfd.endPoint.transform.position);
            i++;
        }

        //Je garde le chemin en m�moire 
        pathData.pathLineRenderer = DEBUG.GetComponent<LineRenderer>();

        DestroyLineList();
    }

    //Destroy le lineRenderer du pathData
    public static void DestroyLinePath(PathData pathData)
    {
        Destroy(pathData.pathLineRenderer);
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
    }

    //Destroy la list de lineRenderer (Puisque je fais un line renderer pour tout le path)
    public static void DestroyLineList()
    {
        foreach(GameObject go in instance.lineRendererDebugList)
        {
            Destroy(go);
        }
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
    #endregion
}
