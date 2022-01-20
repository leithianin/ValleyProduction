using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PathFragmentData 
{
    public IST_PathPoint endPoint;                          //Starting point of the FragmentPath
    public IST_PathPoint startPoint;                        //Ending point of the FragmentPath
    public List<Vector3> path;

    public PathFragmentData(IST_PathPoint nStartPoint, IST_PathPoint nEndPoint, List<Vector3> nPath)
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

    public bool IsSameFragment(PathFragmentData toCheck)
    {
        return ((toCheck.endPoint == endPoint && toCheck.startPoint == startPoint) || (toCheck.startPoint == endPoint || toCheck.endPoint == startPoint));
    }

    public int IsFragmentNeighbours(PathFragmentData possibleNeighbour)
    {
        if(possibleNeighbour.endPoint == endPoint) //On check si le chemin est dans la même direction
        {
            return 1;
        }
        else if (possibleNeighbour.endPoint == startPoint) //On check si le chemin est dansla direction inverse
        {
            return -1;
        }

        return 0;
    }
}
