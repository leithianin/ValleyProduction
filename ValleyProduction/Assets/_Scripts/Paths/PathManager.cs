using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : VLY_Singleton<PathManager>
{
    //Get Data + Update Data

    public static List<PathFragmentData> pathFragmentDataList = new List<PathFragmentData>();
    public static IST_PathPoint previousPathpoint = null;

    [Header("DEBUG")]
    public bool debugMode = false;
    public GameObject DebugLineRenderer;

    //Create PathFragmentData
    public static void PlacePoint(IST_PathPoint pathpoint, Vector3 position)
    {
        if(previousPathpoint != null)
        {
            List<Vector3> allPoints = new List<Vector3>(PathCreationManager.navmeshPositionsList);

            pathFragmentDataList.Add(new PathFragmentData(previousPathpoint, pathpoint, allPoints));

            if (instance.debugMode)
            {
                DebugLine(previousPathpoint.transform.position, pathpoint.transform.position);
            }

            previousPathpoint = pathpoint;
        }
        else
        {
            previousPathpoint = pathpoint;
        }
    }

    public static void DebugLine(Vector3 pos1, Vector3 pos2)
    {
        GameObject DEBUG = Instantiate(instance.DebugLineRenderer);
        DEBUG.GetComponent<LineRenderer>().SetPosition(0, pos1);
        DEBUG.GetComponent<LineRenderer>().SetPosition(1, pos2);
    }
}
