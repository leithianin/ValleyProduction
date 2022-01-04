using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFragmentData : MonoBehaviour
{
    public PathPoint endPoint;                          //Starting point of the FragmentPath
    public PathPoint startPoint;                        //Ending point of the FragmentPath
    public List<Vector3> path;

    public PathFragmentData(PathPoint nStartPoint, PathPoint nEndPoint, List<Vector3> nPath)
    {
        endPoint   = nEndPoint  ;
        startPoint = nStartPoint;

        path = new List<Vector3>(nPath);
    }

    public List<Vector3> GetReversePath()
    {
        List<Vector3> toReturn = new List<Vector3>();

        for (int i = 0; i < path.Count; i++)
        {
            toReturn.Add(path[path.Count - (i + 1)]);
        }
        return toReturn;
    }
}
