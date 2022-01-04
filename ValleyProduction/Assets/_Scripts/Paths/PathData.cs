using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PathData : MonoBehaviour
{
    public PathPoint startPoint;                                                             //Starting point of the path
    public List<PathFragmentData> pathFragment = new List<PathFragmentData>();               //Data of the path portions between 2 marker

    /// <summary>
    /// Check if the path has the point.
    /// </summary>
    /// <param name="toCheck">The point to check.</param>
    /// <returns>Return TRUE if the path has the point. Else return FALSE.</returns>
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

    //Différence avec celle du haut ? Ca retourne true dans tous les cas si ToCheck = startPoint
    public bool ContainsPointWithStart(PathPoint toCheck)
    {
        if (startPoint == toCheck)
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

    //ContainsLandmark --> A mettre ailleurs ? Prends un LandmarkType et check dans les AREA.

    //GetNumberInterestPoint --> Same qu'au dessus

    /// <summary>
    /// Return if the path is usable.
    /// </summary>
    /// <param name="toCheck">Point to check.</param>
    /// <returns>Return TRUE if the path has a startPoint and contain the point toCheck.</returns>
    public bool IsUsable(PathPoint toCheck)
    {
        return startPoint != null && ContainsPoint(toCheck);
    }

    /// <summary>
    /// Get a random destination.
    /// </summary>
    /// <param name="currentPoint">Visitor's currentPoint.</param>
    /// <param name="lastPoint">Visitor's lastPoint he want to reach.</param>
    /// <returns>The new path.</returns>
    public PathFragmentData GetRandomDestination(PathPoint currentPoint, PathPoint lastPoint)
    {
        List<PathFragmentData> availablePaths = new List<PathFragmentData>(GetAvailablesPath(currentPoint, lastPoint));

        if (availablePaths.Count == 0)
        {
            availablePaths = new List<PathFragmentData>(GetAvailablesPath(currentPoint, null));
        }

        return availablePaths[UnityEngine.Random.Range(0, availablePaths.Count)];
    }

    /// <summary>
    /// Return all path available between 2 points.
    /// </summary>
    /// <param name="currentPoint">Visitor's currentPoint.</param>
    /// <param name="lastPoint">Visitor's lastPoint he want to reach.</param>
    /// <returns>A list of paths.</returns>
    private List<PathFragmentData> GetAvailablesPath(PathPoint currentPoint, PathPoint lastPoint)
    {
        List<PathFragmentData> toReturn = new List<PathFragmentData>();
        for (int i = 0; i < pathFragment.Count; i++)
        {
            if (pathFragment[i].startPoint == currentPoint && (pathFragment[i].endPoint != lastPoint || pathFragment.Count == 1))
            {
                toReturn.Add(new PathFragmentData(pathFragment[i].startPoint, pathFragment[i].endPoint, pathFragment[i].path));
            }
            else if ((pathFragment[i].startPoint != lastPoint || pathFragment.Count == 1) && pathFragment[i].endPoint == currentPoint)
            {
                toReturn.Add(new PathFragmentData(pathFragment[i].endPoint, pathFragment[i].startPoint, pathFragment[i].GetReversePath()));
            }
        }

        return toReturn;
    }

    /// <summary>
    /// Remove FragmentData with startPoint and endPoint.
    /// </summary>
    /// <param name="endPoint">FragmentData's endPoint.</param>
    /// <param name="startPoint">FragmentData's startPoint.</param>
    /// <returns>Return PathFragment to Remove if points are valide, else return null.</returns>
    public PathFragmentData RemoveFragment(PathPoint endPoint, PathPoint startPoint)
    {
        PathFragmentData toReturn = null;
        for (int i = pathFragment.Count - 1; i >= 0; i--)
        {
            if (pathFragment[i].startPoint == startPoint && pathFragment[i].endPoint == endPoint)
            {
                toReturn = pathFragment[i];
                pathFragment.RemoveAt(i);
                break;
            }
        }
        return toReturn;
    }
}
