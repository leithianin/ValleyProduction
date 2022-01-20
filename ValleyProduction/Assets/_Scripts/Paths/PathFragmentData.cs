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
    public List<InterestPoint> interestPointList = new List<InterestPoint>();

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

    public void CheckAvailableInterestPoint()
    {
        float f_increment = 1f/4f;
        for(int i = 1; i <= 4; i++)
        {
            Collider[] colliderTab = Physics.OverlapBox(ValleyUtilities.GetVectorPoint3D(startPoint.transform.position, endPoint.transform.position, f_increment*i), new Vector3(1,1,1));

            foreach(Collider c in colliderTab)
            {
                if(c.gameObject.GetComponent<InterestPoint>())
                {
                    AddInterestPoint(c.gameObject.GetComponent<InterestPoint>());
                }
            }
        }
    }

    public void AddInterestPoint(InterestPoint interest_p)
    {
        if(!interestPointList.Contains(interest_p))
        {
            interestPointList.Add(interest_p);
            Debug.Log("Add interest point : " + interest_p.name);
        }
    }
}
