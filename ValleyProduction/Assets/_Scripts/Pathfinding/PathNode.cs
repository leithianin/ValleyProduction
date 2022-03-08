using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour
{
    [SerializeField] private List<PathFragmentData> usableFragments;
    [SerializeField] private List<NodePathData> dataByLandmark;
    private bool isBeingUpdated;

    public Vector3 WorldPosition => transform.position;

    public bool IsBeingUpdated => isBeingUpdated;

    public void AddFragment(PathFragmentData toAdd)
    {
        usableFragments.Add(toAdd);
    }

    public void RemoveFragment(PathFragmentData toRemove)
    {
        usableFragments.Remove(toRemove);
    }

    public PathFragmentData GetMostInterestingPath(BuildTypes target)
    {
        List<PathNode> neighbours = GetNeighbours();

        PathFragmentData toReturn = null;

        float score = 0f;

        for(int i = 0; i < neighbours.Count; i++)
        {
            NodePathData dataToCheck = neighbours[i].GetDataForLandmarkType(target);
            if (1f/dataToCheck.distanceFromLandmark > score)
            {
                score = 1f / dataToCheck.distanceFromLandmark;
                toReturn = usableFragments[i];
            }
        }

        return toReturn;
    }

    public NodePathData GetDataForLandmarkType(BuildTypes landmarkTarget)
    {
        for (int i = 0; i < dataByLandmark.Count; i++)
        {
            if (dataByLandmark[i].landmark == landmarkTarget)
            {
                return dataByLandmark[i];
            }
        }
        return null;
    }

    public void PlaceNode()
    {
        Collider[] colliderTab = Physics.OverlapSphere(transform.position, 0.5f);

        foreach (Collider c in colliderTab)
        {
            InterestPoint foundInterestPoint = c.gameObject.GetComponent<InterestPoint>();
            if (foundInterestPoint != null)
            {
                for (int i = 0; i < dataByLandmark.Count; i++)
                {
                    if (foundInterestPoint.InteractionTypeInInterestPoint().Contains(dataByLandmark[i].landmark))
                    {
                        dataByLandmark[i].distanceFromLandmark = 0;
                        dataByLandmark[i].linkedToLandmark = true;
                    }
                }
            }
        }

        UpdateNode();
    }

    public void DeleteNode()
    {
        List<PathNode> neighbours = GetNeighbours();

        for (int i = 0; i < neighbours.Count; i++)
        {
            if (neighbours[i].HasParent(this))
            {
                neighbours[i].UpdateNode();
            }
        }
    }

    public void UpdateNode()
    {
        isBeingUpdated = true;

        List<PathNode> neighbours = GetNeighbours();

        List<PathNode> toUpdate = new List<PathNode>();

        for (int i = 0; i < neighbours.Count; i++)
        {
            for (int j = 0; j < dataByLandmark.Count; j++)
            {
                NodePathData dataToCheck = neighbours[i].GetDataForLandmarkType(dataByLandmark[j].landmark);

                if (dataToCheck.distanceFromLandmark >= 0 && (dataByLandmark[j].distanceFromLandmark < 0 || dataToCheck.distanceFromLandmark < dataByLandmark[j].distanceFromLandmark))
                {
                    dataByLandmark[j].distanceFromLandmark = Vector3.Distance(WorldPosition, neighbours[i].WorldPosition) + dataToCheck.distanceFromLandmark;
                    dataByLandmark[j].parent = neighbours[i];

                    if (!neighbours[i].IsBeingUpdated)
                    {
                        toUpdate.Add(neighbours[i]);
                    }
                }
                else if (dataByLandmark[j].distanceFromLandmark >= 0 && (dataToCheck.distanceFromLandmark < 0 || dataToCheck.distanceFromLandmark > dataByLandmark[j].distanceFromLandmark))
                {
                    dataToCheck.distanceFromLandmark = Vector3.Distance(WorldPosition, neighbours[i].WorldPosition) + dataByLandmark[j].distanceFromLandmark;
                    dataToCheck.parent = this;

                    if (!neighbours[i].IsBeingUpdated)
                    {
                        toUpdate.Add(neighbours[i]);
                    }
                }
            }
        }

        for (int i = 0; i < toUpdate.Count; i++)
        {
            toUpdate[i].UpdateNode();
        }

        isBeingUpdated = false;
    }

    public bool HasParent(PathNode parentToCheck)
    {
        for (int i = 0; i < dataByLandmark.Count; i++)
        {
            if (dataByLandmark[i].parent == parentToCheck)
            {
                return true;
            }
        }
        return false;
    }

    private List<PathNode> GetNeighbours()
    {
        List<PathNode> toReturn = new List<PathNode>();

        for (int i = 0; i < usableFragments.Count; i++)
        {
            if (usableFragments[i].startPoint.Node == this)
            {
                toReturn.Add(usableFragments[i].endPoint.Node);
            }
            else if (usableFragments[i].endPoint.Node == this)
            {
                toReturn.Add(usableFragments[i].startPoint.Node);
            }
        }

        return toReturn;
    }
}
