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

    //Data
    public IST_PathPoint startPoint;                                                             //Starting point of the path
    public List<PathFragmentData> pathFragment = new List<PathFragmentData>();                   //Data of the path portions between 2 marker

    //PathFragment utilities
    List<PathFragmentData> listGetPathFragment = new List<PathFragmentData>();                   //List of Path's Pathfragment
    public LineRenderer pathLineRenderer;

    //Detector
    [Serializable]
    private class InterestPointDetected
    {
        public InterestPoint interestPoint = null;
        public int detectedCount;
    }

    [SerializeField] private List<InterestPointDetected> interestPointsOnPath = new List<InterestPointDetected>();
    private List<InterestPointDetector> interestPointDetectors = new List<InterestPointDetector>();

    #region PathPoint
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

    public bool ContainsSeveralPoints(IST_PathPoint toCheck)
    {
        int nb = 0;
        foreach(PathFragmentData pfd in pathFragment)
        {
            if(pfd.HasThisPathpoint(toCheck))
            {
                nb++;

                if(nb >=3)
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Return all pathpoints of the pathData
    /// </summary>
    /// <returns></returns>
    public List<IST_PathPoint> GetAllPoints()
    {
        List<IST_PathPoint> toReturn = new List<IST_PathPoint>();

        for (int i = 0; i < pathFragment.Count; i++)
        {
            if (!toReturn.Contains(pathFragment[i].startPoint))
            {
                toReturn.Add(pathFragment[i].startPoint);
            }

            if (!toReturn.Contains(pathFragment[i].endPoint))
            {
                toReturn.Add(pathFragment[i].endPoint);
            }
        }

        return toReturn;
    }

    /// <summary>
    /// Return the last pathpoint of the pathData
    /// </summary>
    /// <returns></returns>
    public IST_PathPoint GetLastPoint()
    {
        return pathFragment[pathFragment.Count - 1].endPoint;
    }

    public List<IST_PathPoint> GetPointNextTo(IST_PathPoint pp)
    {
        List<IST_PathPoint> pathPointNext = new List<IST_PathPoint>();
        List<PathFragmentData> pathFragmentToCheck = new List<PathFragmentData>(GetPathFragments(pp));

        foreach(PathFragmentData pfd in pathFragmentToCheck)
        {
            if(pfd.startPoint == pp)
            {
                pathPointNext.Add(pfd.endPoint);
            }
            else
            {
                pathPointNext.Add(pfd.startPoint);
            }
        }

        return pathPointNext;
    }
    #endregion

    #region PathFragment
    #region Remove PathFragment
    /// <summary>
    /// Use for the Delete with 2 PathFragment (Check wich part need to be delete)
    /// </summary>
    /// <param name="ist_pp"></param>
    public void checkWichFragmentToRemove(IST_PathPoint ist_pp)
    {
        if (pathFragment[0].HasThisStartingPoint(ist_pp)){ RemovePathFragment(pathFragment[0]);}
        else                                             { RemovePathFragment(pathFragment[1]);}
    }

    /// <summary>
    /// Remove fragments deleted and the rest of the path
    /// </summary>
    /// <param name="pathpoint"></param>
    public void RemoveFragmentAndNext(IST_PathPoint pathpoint)
    {
        List<PathFragmentData> listSuppr = new List<PathFragmentData>();                   //List of PathFragmentData to delete
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
            RemovePathFragment(pfd);
        }
    }

    /// <summary>
    /// Take a pathFragment of the path and delete it
    /// </summary>
    /// <param name="toRemove">PathFragment to delete</param>
    public void RemovePathFragment(PathFragmentData toRemove)
    {
        pathFragment.Remove(toRemove);

        toRemove.startPoint.Node.RemoveFragment(toRemove);
        toRemove.endPoint.Node.RemoveFragment(toRemove);
    }
    #endregion

    /// <summary>
    /// Add the pathfragment to the path
    /// </summary>
    /// <param name="toAdd"></param>
    public void AddPathFragment(PathFragmentData toAdd)
    {
        pathFragment.Add(toAdd);

        toAdd.startPoint.Node.AddFragment(toAdd);
        toAdd.endPoint.Node.AddFragment(toAdd);
    }

    /// <summary>
    /// Return pathFragments after a pathpoint 
    /// </summary>
    /// <param name="pathpoint"></param>
    /// <returns></returns>
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

    public List<PathFragmentData> GetPathFragments(IST_PathPoint pp)
    {
        List<PathFragmentData> pathfragmentList = new List<PathFragmentData>();
        
        foreach(PathFragmentData pfd in pathFragment)
        {
            if(pfd.startPoint == pp || pfd.endPoint == pp)
            {
                pathfragmentList.Add(pfd);
            }
        }

        return pathfragmentList;
    }
    #endregion

    #region PathData
    /// <summary>
    /// Safe function that check if the PathData is Empty
    /// </summary>
    public void SafeCheck()
    {
        if(IsPathDataEmpty() || startPoint == null)
        {
            PathManager.DeleteFullPath(this);     
        }
    }

    public bool IsPathDataEmpty()
    {
        if (pathFragment.Count == 0 && startPoint != null)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Update Interest Point of pathDetector when delete PathData
    /// </summary>
    public void DeletePathData()
    {
        for(int i = 0; i < interestPointDetectors.Count; i++)
        {
            interestPointDetectors[i].OnDiscoverInterestPoint -= AddInterestPoint;
            interestPointDetectors[i].OnRemoveInterestPoint -= RemoveInterestPoint;
        }
    }

    #endregion

    #region Detection
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
    #endregion

    #region MultiPath
    public void CheckMultiPath()
    {
        foreach (IST_PathPoint pp in GetAllPoints())
        {
            if(PathManager.HasManyPath(pp))
            {
                List<PathFragmentData> pathFragmentList = GetPathFragments(pp);
                pp.GetManageMultiPath.SetRegisterPathFragment(pathFragmentList);
            }
        }

        
    }
    #endregion

}
