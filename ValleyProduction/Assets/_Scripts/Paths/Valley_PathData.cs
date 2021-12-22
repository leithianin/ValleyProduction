using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Valley_PathData
{
    //public List<PathPoint> pathPoints = new List<PathPoint>(); // Liste des points du chemins.
    public PathPoint startPoint;
    public List<PathFragmentData> pathFragment = new List<PathFragmentData>();
    public Color colorPath;
    public List<ValleyArea> valleyAreaList = new List<ValleyArea>();

    /// <summary>
    /// V�rifie si le chemin poss�de le point.
    /// </summary>
    /// <param name="toCheck">Le point � v�rifier.</param>
    /// <returns>Renvoi TRUE si le chemin poss�de le point. Sinon, renvoi FALSE.</returns>
    public bool ContainsPoint(PathPoint toCheck)
    {
        for (int i = 0; i < pathFragment.Count; i++)
        {
            if (pathFragment[i].endPoint == toCheck || pathFragment[i].startPoint == toCheck)
            {
                return true;
            }
        }

        return false;
    }

    public bool ContainsPointWithStart(PathPoint toCheck)
    {
        if(startPoint == toCheck)
        {
            return true;
        }

        for (int i = 0; i < pathFragment.Count; i++)
        {
            if (pathFragment[i].endPoint == toCheck || pathFragment[i].startPoint == toCheck)
            {
                return true;
            }
        }

        return false;
    }

    public bool ContainsLandmark(LandMarkType wantedLandmark)
    {
        for (int i = 0; i < valleyAreaList.Count; i++)
        {
            if (valleyAreaList[i].ContainsLandmarkType(wantedLandmark))
            {
                return true;
            }
        }
        return false;
    }

    public int GetNumberInterestPoint(InterestPointType wantedInterest)
    {
        int toReturn = 0;
        List<ValleyArea> checkedArea = new List<ValleyArea>();

        for(int i = 0; i < valleyAreaList.Count; i++)
        {
            if (!checkedArea.Contains(valleyAreaList[i]))
            {
                checkedArea.Add(valleyAreaList[i]);
                toReturn += valleyAreaList[i].GetNumberInterestType(wantedInterest);
            }
        }
        return toReturn;
    }

    public bool IsUsable(PathPoint toCheck)
    {
        return startPoint != null && ContainsPoint(toCheck);
    }

    public PathFragmentData GetRandomDestination(PathPoint currentPoint, PathPoint lastPoint)
    {
        List<PathFragmentData> availablePaths = new List<PathFragmentData>(GetAvailablesPath(currentPoint, lastPoint));
        
        if(availablePaths.Count == 0)
        {
            availablePaths = new List<PathFragmentData>(GetAvailablesPath(currentPoint, null));
        }

        return availablePaths[UnityEngine.Random.Range(0, availablePaths.Count)];
    }

    private List<PathFragmentData> GetAvailablesPath(PathPoint currentPoint, PathPoint lastPoint)
    {
        List<PathFragmentData> toReturn = new List<PathFragmentData>();
        for (int i = 0; i < pathFragment.Count; i++)
        {
            if (pathFragment[i].startPoint == currentPoint && (pathFragment[i].endPoint != lastPoint || pathFragment.Count == 1))
            {
                toReturn.Add(new PathFragmentData(pathFragment[i].startPoint, pathFragment[i].endPoint, pathFragment[i].path, pathFragment[i].line));
            }
            else if ((pathFragment[i].startPoint != lastPoint || pathFragment.Count == 1) && pathFragment[i].endPoint == currentPoint)
            {
                toReturn.Add(new PathFragmentData(pathFragment[i].endPoint, pathFragment[i].startPoint, pathFragment[i].GetReversePath(), pathFragment[i].line));
            }
        }

        return toReturn;
    }

    public PathFragmentData RemoveFragment(PathPoint endPoint, PathPoint startPoint)
    {
        PathFragmentData toReturn = null;
        for(int i = pathFragment.Count-1; i >= 0 ; i--)
        {
            if(pathFragment[i].startPoint == startPoint && pathFragment[i].endPoint == endPoint)
            {
                toReturn = pathFragment[i];
                pathFragment.RemoveAt(i);
                break;
            }
        }
        return toReturn;
    }
}
