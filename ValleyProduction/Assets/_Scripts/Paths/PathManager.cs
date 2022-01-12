using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : VLY_Singleton<PathManager>
{
    //Get Data + Update Data

    public List<PathFragmentData> pathFragmentDataList = new List<PathFragmentData>();
    public List<IST_PathPoint> pathpointList = new List<IST_PathPoint>();
    public static IST_PathPoint previousPathpoint = null;

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

    public static bool CanDeletePoint(IST_PathPoint ist_pp)
    {
        if(ist_pp != instance.pathpointList[instance.pathpointList.Count-1])
        {
            return false;
        }

        DeletePoint(ist_pp);
        return true;
    }

    public static void DeletePoint(IST_PathPoint ist_pp)
    {
        if (instance.pathFragmentDataList.Count != 0)
        {
            instance.pathFragmentDataList.RemoveAt(instance.pathFragmentDataList.Count - 1);
        }
        instance.pathpointList.Remove(ist_pp);

        if (instance.pathFragmentDataList.Count != 0)
        {
            previousPathpoint = instance.pathpointList[instance.pathpointList.Count - 1];
        }
    }

    public static void ModifyPoint()
    {

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
