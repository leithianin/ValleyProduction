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
    private static List<GameObject> lineRendererList = new List<GameObject>();

    private void Update()
    {
        if (instance.debugMode)
        {
            DebugCheck();
        }
        else
        {
            debugCheck = false;
            DebugClear();       
        }
    }

    //Create PathFragmentData
    public static void PlacePoint(IST_PathPoint pathpoint, Vector3 position)
    {
        instance.pathpointList.Add(pathpoint);

        if(previousPathpoint != null)
        {
            List<Vector3> allPoints = new List<Vector3>(PathCreationManager.navmeshPositionsList);
            instance.pathFragmentDataList.Add(new PathFragmentData(previousPathpoint, pathpoint, allPoints));
            previousPathpoint = pathpoint;
        }
        else
        {
            previousPathpoint = pathpoint;
        }
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
            Debug.Log("return 1");
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
                Debug.Log("return 2");
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
                Debug.Log("return 3");
                return false;
            }
        }

        DeletePoint(ist_pp);
        Debug.Log("return 4");
        return true;
    }

    public static void DeletePoint(IST_PathPoint ist_pp)
    {
        if (instance.pathFragmentDataList.Count != 0)
        {
            instance.currentPathData.RemoveFragment(instance.pathFragmentDataList[instance.pathFragmentDataList.Count-1]);
            instance.pathFragmentDataList.RemoveAt(instance.pathFragmentDataList.Count - 1);
        }
        instance.pathpointList.RemoveAt(instance.pathpointList.Count-1);

        if (instance.pathFragmentDataList.Count != 0)
        {
            previousPathpoint = instance.pathpointList[instance.pathpointList.Count - 1];
        }
    }

    public static void CreatePathData()
    {
        if (instance.pathpointList.Count > 0)
        {
            if (instance.currentPathData != null)
            {
                foreach (PathFragmentData pfd in instance.pathFragmentDataList)
                {
                    if (!instance.currentPathData.pathFragment.Contains(pfd))
                    {
                        instance.currentPathData.pathFragment.Add(pfd);
                    }
                }

                instance.pathFragmentDataList.Clear();
                instance.pathpointList.Clear();
                previousPathpoint = null;
            }
            else
            {
                PathData newPathData = new PathData();
                newPathData.pathFragment = new List<PathFragmentData>(instance.pathFragmentDataList);
                newPathData.startPoint = instance.pathpointList[0];
                instance.pathDataList.Add(newPathData);
                instance.pathFragmentDataList.Clear();
                instance.pathpointList.Clear();
                previousPathpoint = null;
            }
        }
    }

    /// <summary>
    /// Selectionner un chemin à partir d'un pathpoint.
    /// </summary>
    /// <param name="pathpoint"></param>
    public static void SelectPath(IST_PathPoint pathpoint)
    {
        CreatePathData();                                                           //Si il selectionne un autre chemin, on save celui en cours quand même

        foreach(PathData pd in instance.pathDataList)
        {
            if(pd.ContainsPoint(pathpoint))
            {
                instance.currentPathData = pd;

                //Il faut trouver les pathpoints
                for (int i = 0; i <= pd.pathFragment.Count-1; i++)
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

        previousPathpoint = instance.pathpointList[instance.pathpointList.Count - 1];
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

        /*
        if (!pathpointList.Contains(pfd.startPoint))
        {
            pathpointList.Add(pfd.startPoint);
        }
        if (!pathpointList.Contains(pfd.endPoint))
        {
            pathpointList.Add(pfd.endPoint);
        }
        */
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
    public static void DebugCheck()
    {
        if (!debugCheck)
        {
            foreach (PathFragmentData pfd in instance.pathFragmentDataList)
            {
                DebugLine(pfd.startPoint.transform.position, pfd.endPoint.transform.position);
            }

            debugCheck = true;
        }
    }

    public static void DebugClear()
    {
        foreach(GameObject go in lineRendererList)
        {
            Destroy(go);
        }
    }

    public static void DebugLine(Vector3 pos1, Vector3 pos2)
    {
        GameObject DEBUG = Instantiate(instance.DebugLineRenderer);
        lineRendererList.Add(DEBUG);
        DEBUG.GetComponent<LineRenderer>().SetPosition(0, pos1);
        DEBUG.GetComponent<LineRenderer>().SetPosition(1, pos2);
    }
    #endregion
}
