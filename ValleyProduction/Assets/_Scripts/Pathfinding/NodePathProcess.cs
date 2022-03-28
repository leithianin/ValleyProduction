using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePathProcess : VLY_Singleton<NodePathProcess>
{
    private List<PathNode> nodesNextToLandmark = new List<PathNode>();

    private List<PathNode> updatedNodes = new List<PathNode>();

    private List<PathNode> nodesToUpdate = new List<PathNode>();

    public static List<PathNode> GetAllObjectiveNodes => instance.nodesNextToLandmark;

    private static bool isProcessingPath = false;

    private Action OnUpdateNode;

    public static void CallOnUpdateNode(Action callback)
    {
        instance.OnUpdateNode += callback;
    }

    public static void UncallOnUpdateNode(Action callback) 
    {
        instance.OnUpdateNode -= callback;
    }

    public static void AddNodeNextLandmark(PathNode toAdd)
    {
        if (!instance.nodesNextToLandmark.Contains(toAdd))
        {
            instance.nodesNextToLandmark.Add(toAdd);
        }
    }

    public static void RemoveNodeNextLandmark(PathNode toRemove)
    {
        if (instance.nodesNextToLandmark.Contains(toRemove))
        {
            instance.nodesNextToLandmark.Remove(toRemove);
        }
    }

    public static void UpdateAllNodes()
    {
        if (!isProcessingPath)
        {
            isProcessingPath = true;

            instance.UpdateFirstLandmarkNode(new List<PathNode>(instance.nodesNextToLandmark));

            isProcessingPath = false;

            instance.OnUpdateNode?.Invoke();
        }
    }

    private void EndUpdatePath()
    {
        isProcessingPath = false;
    }

    private void UpdateFirstLandmarkNode(List<PathNode> leftToUpdates)
    {
        if(leftToUpdates.Count > 0)
        {
            LandmarkType landmarkToCheck = leftToUpdates[0].GetLandmarkNextTo();

            UpdateNodeForLandmark(leftToUpdates[0], landmarkToCheck);
            leftToUpdates.RemoveAt(0);

            for (int i = 0; i < updatedNodes.Count; i++)
            {
                updatedNodes[i].ResetUpdateState();
            }

            updatedNodes.Clear();
            nodesToUpdate.Clear();

            UpdateFirstLandmarkNode(new List<PathNode>(leftToUpdates));

            //TimerManager.CreateRealTimer(Time.deltaTime, () => UpdateFirstLandmarkNode(new List<PathNode>(leftToUpdates)));
        }
        else
        {
            EndUpdatePath();
        }
    }

    public static void UpdateNodeForLandmark(PathNode toUpdate, LandmarkType toCheck)
    {
        int security = 1000;

        instance.nodesToUpdate.Add(toUpdate);

        while (security > 0 && instance.nodesToUpdate.Count > 0)
        {
            instance.UpdateForLandmark(instance.nodesToUpdate[0], toCheck);
            security--;
        }

        if(security <= 0)
        {
            Debug.LogError("Node path calcul took too long !!");
        }
    }

    private void UpdateForLandmark(PathNode toUpdate, LandmarkType toCheck)
    {
        toUpdate.UpdateSelfData(toCheck);

        updatedNodes.Add(toUpdate);
        nodesToUpdate.Remove(toUpdate);

        List<PathNode> neighbours = toUpdate.GetNeighbours();

        foreach (PathNode n in neighbours)
        {
            if (!updatedNodes.Contains(n) && !nodesToUpdate.Contains(n))
            {
                nodesToUpdate.Add(n);
            }
        }
    }

    public static void SetNodeUpdating(PathNode toSet)
    {
        instance.updatedNodes.Add(toSet);
    }
}
