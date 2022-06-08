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
    private bool isBeingDeleted;

    [SerializeField] private bool isOpen = true;

    public Action<PathNode> OnUpdateNode;
    public Action<PathNode> OnDeleteNode;

    public Vector3 WorldPosition => transform.position;

    public bool IsBeingUpdated => isBeingUpdated;

    public bool IsBeingDeleted => isBeingDeleted;

    public bool IsOpen => isOpen;

    public void ResetUpdateState()
    {
        isBeingUpdated = false;
    }

    /// <summary>
    /// Add a PathFragmentData to the list of usable path fragment.
    /// </summary>
    /// <param name="toAdd">The PAthFragmentData to add.</param>
    public void AddFragment(PathFragmentData toAdd)
    {
        if (!usableFragments.Contains(toAdd))
        {
            usableFragments.Add(toAdd);
        }
    }

    /// <summary>
    /// Remove a PathFragmentData to the list of usable path fragment.
    /// </summary>
    /// <param name="toRemove">The PAthFragmentData to remove.</param>
    public void RemoveFragment(PathFragmentData toRemove)
    {
        usableFragments.Remove(toRemove);
    }
 

    #region Data Management
    public void SetDatas()
    {
        dataByLandmark = new List<NodePathData>();
        foreach (CPN_IsLandmark landmark in VLY_LandmarkManager.AllLandmarks)
        {
            dataByLandmark.Add(new NodePathData(landmark));
        }
    }

    /// <summary>
    /// Check if the PathNode is next to a Landmark and Update the node.
    /// </summary>
    public void PlaceNode()
    {
        SetDatas();

        Collider[] colliderTab = Physics.OverlapSphere(transform.position, 0.5f);

        foreach (Collider c in colliderTab)
        {
            CPN_IsLandmark foundInterestPoint = c.gameObject.GetComponent<CPN_IsLandmark>();
            if (foundInterestPoint != null)
            {
                for (int i = 0; i < dataByLandmark.Count; i++)
                {
                    if (foundInterestPoint == dataByLandmark[i].landmark)
                    {
                        foundInterestPoint.AddPointToLandmark(this);
                        dataByLandmark[i].distanceFromLandmark = 0;
                        dataByLandmark[i].linkedToLandmark = true;
                        NodePathProcess.AddNodeNextLandmark(this);
                    }
                }
            }
        }
    }

    public void DeleteNode()
    {
        NodePathProcess.RemoveNodeNextLandmark(this);

        ResetNodeData();
    }

    public bool UpdateSelfData(CPN_IsLandmark wantedLandmark)
    {
        bool toReturn = false;

        List<PathNode> neighbours = GetNeighbours();

        for (int i = 0; i < neighbours.Count; i++)
        {
            if (!neighbours[i].IsBeingDeleted)
            {
                NodePathData selfToCheck = GetDataForLandmarkType(wantedLandmark);
                if (selfToCheck != null)
                {
                    NodePathData neighbourToCheck = neighbours[i].GetDataForLandmarkType(wantedLandmark);

                    if (neighbourToCheck != null)
                    {
                        float distanceFromNeighbour = Vector3.Distance(WorldPosition, neighbours[i].WorldPosition);

                        if (neighbourToCheck.distanceFromLandmark >= 0 && (selfToCheck.distanceFromLandmark < 0 || neighbourToCheck.distanceFromLandmark < selfToCheck.distanceFromLandmark))
                        {
                            selfToCheck.distanceFromLandmark = distanceFromNeighbour + neighbourToCheck.distanceFromLandmark;
                            selfToCheck.parent = neighbours[i];
                        }
                    }
                }
            }
        }

        if(toReturn)
        {
            OnUpdateNode?.Invoke(this);
        }

        return toReturn;
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

        CheckNeighboursOnDelete();
    }
    public void ResetNodeData()
    {
        for (int i = 0; i < dataByLandmark.Count; i++)
        {
            if (!dataByLandmark[i].linkedToLandmark)
            {
                dataByLandmark[i].distanceFromLandmark = -1;
                dataByLandmark[i].parent = null;
            }
        }

        CheckNeighboursOnDelete();

        OnDeleteNode?.Invoke(this);
    }

    /// <summary>
    /// Tell the neighbours that the Node has been deleted.
    /// </summary>
    public void CheckNeighboursOnDelete()
    {
        isBeingDeleted = true;

        List<PathNode> neighbours = GetNeighbours();

        for (int i = 0; i < neighbours.Count; i++)
        {
            if (neighbours[i].HasParent(this))
            {
                neighbours[i].UpdateFromDeletedNode(this);
            }
        }

        isBeingDeleted = false;
    }


    #endregion

    #region Utilities
    /// <summary>
    /// Check if the node has "parentToCheck" as a parent for one of the Landmark.
    /// </summary>
    /// <param name="parentToCheck">The parent to search for.</param>
    /// <returns>True if the variable is used as a parent.</returns>
    public bool HasParent(PathNode parentToCheck)
    {
        return GetDataLandmarkWithParent(parentToCheck).Count > 0;
    }

    public CPN_IsLandmark GetLandmarkNextTo()
    {
        for(int i = 0; i < dataByLandmark.Count; i++)
        {
            if(dataByLandmark[i].linkedToLandmark)
            {
                return dataByLandmark[i].landmark;
            }
        }

        return null;
    }

    private List<NodePathData> GetDataLandmarkWithParent(PathNode parentToSearch)
    {
        List<NodePathData> toReturn = new List<NodePathData>();

        for (int i = 0; i < dataByLandmark.Count; i++)
        {
            if (dataByLandmark[i].parent == parentToSearch)
            {
                toReturn.Add(dataByLandmark[i]);
            }
        }

        return toReturn;
    }

    /// <summary>
    /// Search for all neighbours PathNode.
    /// </summary>
    /// <returns>A list of all the PathNode linked to the node.</returns>
    public List<PathNode> GetNeighbours()
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

    public bool HasNeighboursLinkedToSpawn()
    {
        List<PathNode> neighbours = GetNeighbours();

        for (int i = 0; i < neighbours.Count; i++)
        {
            if (neighbours[i].HasParent(this))
            {
                return true;
            }
        }
        return false;
    }

    public bool IsNextToLandmark(CPN_IsLandmark landmarkToSearch)
    {
        NodePathData dataToCheck = GetDataForLandmarkType(landmarkToSearch);
        if (dataToCheck.linkedToLandmark)
        {
            return true;
        }
        return false;
    }

    public bool HasValidPathForLandmark(CPN_IsLandmark landmarkToSearch)
    {
        NodePathData dataToCheck = GetDataForLandmarkType(landmarkToSearch);
        if (dataToCheck.parent != null || dataToCheck.linkedToLandmark)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Search for a NodePathData that contains the LandmarkType wanted.
    /// </summary>
    /// <param name="landmarkTarget">The LandmarkType to search for.</param>
    /// <returns>Return the NodePathData that contains the LandmarkType. Return null if the LandmarkType is unknown.</returns>
    public NodePathData GetDataForLandmarkType(CPN_IsLandmark landmarkTarget)
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

    public void ClosePath()
    {
        isOpen = false;
    }

    public void OpenPath()
    {
        isOpen = true;
    }

    #endregion

    #region Score Calculation

    /// <summary>
    /// Search for the most interesting path depending on several datas.
    /// </summary>
    /// <param name="target">The Landmark to search for.</param>
    /// <returns>The PathFragmentData for the visitor to follow.</returns>
    public PathFragmentData GetMostInterestingPath(CPN_IsLandmark target, PathFragmentData currentUsedFragment, List<SatisfactorType> likedTypes, List<SatisfactorType> hatedTypes, List<PathFragmentData> toIgnore)
    {
        PathFragmentData toReturn = null;

        float score = -1f;

        for(int i = 0; i < usableFragments.Count; i++)
        {
            float nScore = -1;

            if (currentUsedFragment != null && currentUsedFragment.IsSameFragment(usableFragments[i]))
            {
                nScore = -0.5f;
            }
            else
            {
                if (toIgnore.Contains(usableFragments[i]))
                {
                    nScore = CalculateScore(usableFragments[i], target, new List<SatisfactorType>(), new List<SatisfactorType>());
                }
                else
                {
                    nScore = CalculateScore(usableFragments[i], target, likedTypes, hatedTypes);
                }
            }

            if(nScore > score)
            {
                score = nScore;
                toReturn = usableFragments[i];
            }
        }

        if (toReturn == null && usableFragments.Count > 0)
        {
            toReturn = usableFragments[UnityEngine.Random.Range(0, usableFragments.Count)];
        }

        return toReturn;
    }

    private float CalculateScore(PathFragmentData fragmentToCalculate, CPN_IsLandmark landmarkWanted, List<SatisfactorType> likedTypes, List<SatisfactorType> hatedTypes)
    {
        NodePathData dataToCheck = null;

        float distanceScore = 100f;

        float attractivityScore = 0;

        bool fragmentOpen = true;

        if (fragmentToCalculate.startPoint.Node != this)
        {
            dataToCheck = fragmentToCalculate.startPoint.Node.GetDataForLandmarkType(landmarkWanted);
            fragmentOpen = fragmentToCalculate.startPoint.Node.IsOpen;
        }
        else
        {
            dataToCheck = fragmentToCalculate.endPoint.Node.GetDataForLandmarkType(landmarkWanted);
            fragmentOpen = fragmentToCalculate.endPoint.Node.IsOpen;
        }

        if(dataToCheck != null)
        {
            if (dataToCheck.distanceFromLandmark < 100)
            {
                distanceScore -= dataToCheck.distanceFromLandmark;
            }
            else
            {
                distanceScore -= 100f;
            }
        }

        for(int i = 0; i < fragmentToCalculate.InterestPointsOnFragment.Count; i++)
        {
            attractivityScore += fragmentToCalculate.InterestPointsOnFragment[i].GetAttractivityScore(likedTypes, hatedTypes);
        }

        if(distanceScore >= 100)
        {
            attractivityScore = 150;
        }
        else if (attractivityScore > 100)
        {
            attractivityScore = 100;
        }
        else if (attractivityScore < 0)
        {
            attractivityScore = 0;
        }

        if (fragmentOpen)
        {
            return attractivityScore + distanceScore;
        }
        else
        {
            return -5f;
        }
    }
    #endregion
}
