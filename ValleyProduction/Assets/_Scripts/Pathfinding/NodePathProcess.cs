using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePathProcess : VLY_Singleton<NodePathProcess>
{
    private List<PathNode> updatedNodes = new List<PathNode>();

    public static void UpdateNode(PathNode toUpdate)
    {
        instance.updatedNodes.Add(toUpdate);
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
