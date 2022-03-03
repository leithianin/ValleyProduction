using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PathData
{
    //Informations
    public string name = string.Empty;
    public Color color;
    public float difficulty = 5f;
    public bool isDisconnected = false;

    public IST_PathPoint startPoint;                                                             //Starting point of the path
    public List<PathFragmentData> pathFragment = new List<PathFragmentData>();               //Data of the path portions between 2 marker

    List<PathFragmentData> listSuppr = new List<PathFragmentData>();

    List<PathFragmentData> listGetPathFragment = new List<PathFragmentData>();

    [Serializable]
    private class InterestPointDetected
    {
        public InterestPoint interestPoint = null;
        public int detectedCount;
    }

    [SerializeField] private List<InterestPointDetected> interestPointsOnPath = new List<InterestPointDetected>();

    //Debug Path --> J'ai besoin de savoir � qui il appartient lors de la suppression
    public LineRenderer pathLineRenderer;

    private List<InterestPointDetector> interestPointDetectors = new List<InterestPointDetector>();

    /// <summary>
    /// Check if the path has the point.
    /// </summary>
    /// <param name="toCheck">The point to check.</param>
    /// <returns>Return TRUE if the path has the point. Else return FALSE.</returns>
    public bool ContainsPoint(IST_PathPoint toCheck)
    {
        for (int i = 0; i < pathFragment.Count; i++)
        {
            if (pathFragment[i].endPoint == toCheck || pathFragment[i].startPoint == toCheck)
            {
                return true;
            }
        }

        if(startPoint == toCheck)
        {
            return true;
        }

        return false;
    }

    //Diff�rence avec celle du haut ? Ca retourne true dans tous les cas si ToCheck = startPoint
    public bool ContainsPointWithStart(IST_PathPoint toCheck)
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
    public bool IsUsable(IST_PathPoint toCheck)
    {
        return startPoint != null && ContainsPoint(toCheck);
    }

    /// <summary>
    /// Get a random destination.
    /// </summary>
    /// <param name="currentPoint">Visitor's currentPoint.</param>
    /// <param name="lastPoint">Visitor's lastPoint he want to reach.</param>
    /// <returns>The new path.</returns>
    public PathFragmentData GetRandomDestination(IST_PathPoint currentPoint, IST_PathPoint lastPoint)
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
    private List<PathFragmentData> GetAvailablesPath(IST_PathPoint currentPoint, IST_PathPoint lastPoint)
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
    /*
    public PathFragmentData RemoveFragment(IST_PathPoint endPoint, IST_PathPoint startPoint)
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
    }*/

    public void checkWichFragmentToRemove(IST_PathPoint ist_pp)
    {
        if (pathFragment[0].HasThisStartingPoint(ist_pp))
        {
            pathFragment.RemoveAt(0);
        }
        else
        {
            pathFragment.RemoveAt(1);
        }
    }

    public void RemoveFragmentAndNext(IST_PathPoint pathpoint)
    {
        listSuppr.Clear();
        bool endingPointReach = false;

        for (int i = 0; i < pathFragment.Count;i++)
        {
            if(endingPointReach)  
            {
                listSuppr.Add(pathFragment[i]);
            }
            else if(pathFragment[i].HasThisEndingPoint(pathpoint)) 
            {
                listSuppr.Add(pathFragment[i]);
                endingPointReach = true;
            }
            else if (pathFragment[i].HasThisStartingPoint(pathpoint))
            {
                listSuppr.Add(pathFragment[i]);
            }
        }

        foreach (PathFragmentData pfd in listSuppr)
        {
            pathFragment.Remove(pfd);
        }
    }

    public List<PathFragmentData> GetAllNextPathFragment(IST_PathPoint pathpoint)
    {
        listGetPathFragment.Clear();
        bool saveNextPfd = false;

        for(int i = 0; i < pathFragment.Count; i++)
        {
            if(saveNextPfd)                                     { listGetPathFragment.Add(pathFragment[i]);}
            if(pathFragment[i].HasThisStartingPoint(pathpoint)) { saveNextPfd = true       ;}
        }

        return listGetPathFragment;
    }

    public void RemoveFragment(PathFragmentData pfd)
    {
        pathFragment.Remove(pfd);
    }

    public IST_PathPoint GetLastPoint()
    {
        return pathFragment[pathFragment.Count - 1].endPoint;
    }

    public void SafeCheck()
    {
        if(IsPathDataEmpty() || startPoint == null)
        {
            PathManager.DeleteFullPath(this);     
        }
    }

    public void DeletePathData()
    {
        for(int i = 0; i < interestPointDetectors.Count; i++)
        {
            interestPointDetectors[i].OnDiscoverInterestPoint -= AddInterestPoint;
            interestPointDetectors[i].OnRemoveInterestPoint -= RemoveInterestPoint;
        }
    }

    public bool IsPathDataEmpty()
    {
        if(pathFragment.Count == 0 && startPoint != null)
        {
            return true;
        }
        return false;
    }

    public void AddInterestPointDetector(InterestPointDetector detector)
    {
        if(!interestPointDetectors.Contains(detector))
        {
            interestPointDetectors.Add(detector);
            detector.OnDiscoverInterestPoint += AddInterestPoint;
            detector.OnRemoveInterestPoint += RemoveInterestPoint;
        }
    }

    public void RemoveInterestPointDetector(InterestPointDetector detector)
    {
        if (interestPointDetectors.Contains(detector))
        {
            interestPointDetectors.Remove(detector);
            detector.OnDiscoverInterestPoint -= AddInterestPoint;
            detector.OnRemoveInterestPoint -= RemoveInterestPoint;
        }
    }

    private int ContrainsInterestPoint(InterestPoint toCheck)
    {
        int toReturn = -1;

        for(int i = 0; i < interestPointsOnPath.Count; i++)
        {
            if(interestPointsOnPath[i].interestPoint == toCheck)
            {
                toReturn = i;
                break;
            }
        }

        return toReturn;
    }

    private void AddInterestPoint(InterestPoint newInterestPoint)
    {
        int containerIndex = ContrainsInterestPoint(newInterestPoint);
        if (containerIndex >= 0)
        {
            interestPointsOnPath[containerIndex].detectedCount++;
        }
        else
        {
            Debug.Log("Add point");
            interestPointsOnPath.Add(new InterestPointDetected());
            interestPointsOnPath[interestPointsOnPath.Count - 1].detectedCount = 1;
            interestPointsOnPath[interestPointsOnPath.Count - 1].interestPoint = newInterestPoint;
        }
    }

    private void RemoveInterestPoint(InterestPoint newInterestPoint)
    {
        int containerIndex = ContrainsInterestPoint(newInterestPoint);
        if (containerIndex >= 0)
        {
            interestPointsOnPath[containerIndex].detectedCount--;
            if(interestPointsOnPath[containerIndex].detectedCount <= 0)
            {
                Debug.Log("Remove point");
                interestPointsOnPath.RemoveAt(containerIndex);
            }
        }

    }
}
