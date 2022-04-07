using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PathFragmentData 
{
    public IST_PathPoint endPoint;                          //Starting point of the FragmentPath
    public IST_PathPoint startPoint;                        //Ending point of the FragmentPath
    public List<Vector3> path = new List<Vector3>();
    public List<GameObject> pathRendererObject = new List<GameObject>();
    [SerializeField] private List<InterestPoint> interestPointList = new List<InterestPoint>();
    public List<InterestPointDetector> interestPointDetectors = new List<InterestPointDetector>();

    public List<InterestPoint> InterestPointsOnFragment => interestPointList;

    public PathFragmentData(IST_PathPoint nStartPoint, IST_PathPoint nEndPoint, List<Vector3> nPath)
    {
        endPoint   = nEndPoint  ;
        startPoint = nStartPoint;

        path = new List<Vector3>(nPath);  
    }

    public void DeleteFragmentData()
    {
        while(interestPointDetectors.Count > 0)
        {
            RemoveInterestPointDetector(interestPointDetectors[0]);
        }

        if (startPoint != null)
        {
            startPoint.Node.RemoveFragment(this);
        }

        if (endPoint != null)
        {
            endPoint.Node.RemoveFragment(this);
        }
    }

    public bool HasThisPathpoint(IST_PathPoint pp)
    {
        if(endPoint == pp || startPoint == pp)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool HasThisStartingPoint(IST_PathPoint pp)
    {
        if(startPoint == pp) {return true;}

        return false;
    }

    public bool HasThisEndingPoint(IST_PathPoint pp)
    {
        if (endPoint == pp) {return true;}

        return false;
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
        return ((toCheck.endPoint == endPoint && toCheck.startPoint == startPoint) || (toCheck.startPoint == endPoint && toCheck.endPoint == startPoint));
    }

    public int IsFragmentNeighbours(PathFragmentData possibleNeighbour)
    {
        if(possibleNeighbour.endPoint == startPoint) //On check si le chemin est dans la m�me direction
        {
            return 1;
        }
        else if (possibleNeighbour.endPoint == endPoint) //On check si le chemin est dansla direction inverse
        {
            return -1;
        }

        return 0;
    }

    public void AddInterestPoint(InterestPoint interest_p)
    {
        if(!interestPointList.Contains(interest_p))
        {
            interestPointList.Add(interest_p);
            Debug.Log("Add interest point : " + interest_p.name);
        }
    }

    public void RemoveInterestPoint(InterestPoint toRemove)
    {
        if (interestPointList.Contains(toRemove))
        {
            interestPointList.Remove(toRemove);
        }
    }

    public void AddInterestPointDetector(InterestPointDetector detector)
    {
        if (!interestPointDetectors.Contains(detector))
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

}
