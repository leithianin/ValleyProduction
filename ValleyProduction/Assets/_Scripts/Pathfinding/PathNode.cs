using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PathNode : MonoBehaviour
{
    [SerializeField] private List<PathFragmentData> usableFragments;
    [SerializeField] private List<NodePathData> dataByLandmark;
    private bool isBeingUpdated;

    public Vector3 WorldPosition => transform.position;

    public bool IsBeingUpdated => isBeingUpdated;

    /// <summary>
    /// Add a PathFragmentData to the list of usable path fragment.
    /// </summary>
    /// <param name="toAdd">The PAthFragmentData to add.</param>
    public void AddFragment(PathFragmentData toAdd)
    {
        usableFragments.Add(toAdd);
    }

    /// <summary>
    /// Remove a PathFragmentData to the list of usable path fragment.
    /// </summary>
    /// <param name="toRemove">The PAthFragmentData to remove.</param>
    public void RemoveFragment(PathFragmentData toRemove)
    {
        usableFragments.Remove(toRemove);
    }

    /// <summary>
    /// Search for the most interesting path depending on several datas.
    /// </summary>
    /// <param name="target">The Landmark to search for.</param>
    /// <returns>The PathFragmentData for the visitor to follow.</returns>
    public PathFragmentData GetMostInterestingPath(LandmarkType target)
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

        if(toReturn == null)
        {
            toReturn = usableFragments[UnityEngine.Random.Range(0, usableFragments.Count)];
        }

        return toReturn;
    }

    /// <summary>
    /// Search for a NodePathData that contains the LandmarkType wanted.
    /// </summary>
    /// <param name="landmarkTarget">The LandmarkType to search for.</param>
    /// <returns>Return the NodePathData that contains the LandmarkType. Return null if the LandmarkType is unknown.</returns>
    public NodePathData GetDataForLandmarkType(LandmarkType landmarkTarget)
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

    /// <summary>
    /// Check if the PathNode is next to a Landmark and Update the node.
    /// </summary>
    public void PlaceNode()
    {
        Collider[] colliderTab = Physics.OverlapSphere(transform.position, 0.5f);

        foreach (Collider c in colliderTab)
        {
            CPN_IsLandmark foundInterestPoint = c.gameObject.GetComponent<CPN_IsLandmark>();
            if (foundInterestPoint != null)
            {
                for (int i = 0; i < dataByLandmark.Count; i++)
                {
                    if (foundInterestPoint.Type == dataByLandmark[i].landmark)
                    {
                        dataByLandmark[i].distanceFromLandmark = 0;
                        dataByLandmark[i].linkedToLandmark = true;
                    }
                }
            }
        }

        UpdateNode();
    }

    /// <summary>
    /// Tell the neighbours that the Node has been deleted.
    /// </summary>
    public void DeleteNode()
    {
        List<PathNode> neighbours = GetNeighbours();

        for (int i = 0; i < neighbours.Count; i++)
        {
            if (neighbours[i].HasParent(this))
            {
                neighbours[i].UpdateFromDeletedNode(this);
            }
        }
    }

    public void UpdateFromDeletedNode(PathNode deletedNode)
    {
        for (int i = 0; i < dataByLandmark.Count; i++)
        {
            if (dataByLandmark[i].parent == deletedNode && !dataByLandmark[i].linkedToLandmark)
            {
                dataByLandmark[i].distanceFromLandmark = -1;
                dataByLandmark[i].parent = null;
            }
        }

        DeleteNode();

        UpdateNode();
    }

    /// <summary>
    /// Update the node for all known LandmarkType.
    /// </summary>
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

    /// <summary>
    /// Check if the node has "parentToCheck" as a parent for one of the Landmark.
    /// </summary>
    /// <param name="parentToCheck">The parent to search for.</param>
    /// <returns>True if the variable is used as a parent.</returns>
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

    /// <summary>
    /// Search for all neighbours PathNode.
    /// </summary>
    /// <returns>A list of all the PathNode linked to the node.</returns>
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

    private void OnDrawGizmos()
    {
        if (Selection.activeGameObject != transform.gameObject)
        {
            return;
        }

        int i = 0;

        PathNode parent = this;

        while (i < 100 && parent != null)
        {
            i++;
            PathNode lastParent = parent;
            parent = parent.dataByLandmark[1].parent;
            if (parent != null)
            {
                Gizmos.DrawLine(lastParent.WorldPosition, parent.WorldPosition);
            }
        }
    }
}
