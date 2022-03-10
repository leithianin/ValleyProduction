using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePathProcess : VLY_Singleton<NodePathProcess>
{
    private List<PathNode> nodesNextToLandmark = new List<PathNode>();

    private List<PathNode> updatedNodes = new List<PathNode>();

    public static List<PathNode> GetAllObjectiveNodes => instance.nodesNextToLandmark;

    private static bool isProcessingPath = false;

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
            Debug.Log("UpdateAll");

            isProcessingPath = true;

            instance.UpdateFirstLandmarkNode(new List<PathNode>(instance.nodesNextToLandmark));

            isProcessingPath = false;
        }
    }

    private void UpdateFirstLandmarkNode(List<PathNode> leftToUpdates)
    {
        if(leftToUpdates.Count > 0)
        {
            UpdateNode(leftToUpdates[0]);
            leftToUpdates.RemoveAt(0);

            TimerManager.CreateRealTimer(Time.deltaTime, () => UpdateFirstLandmarkNode(new List<PathNode>(leftToUpdates)));
        }
    }

    public static void UpdateNode(PathNode toUpdate)
    {
        toUpdate.UpdateNode();
    }

    public static void SetNodeUpdating(PathNode toSet)
    {
        instance.updatedNodes.Add(toSet);
    }

    private void LateUpdate()
    {
        for(int i = 0; i < updatedNodes.Count; i++)
        {
            updatedNodes[i].ResetUpdateState();
        }

        updatedNodes.Clear();
    }
}
