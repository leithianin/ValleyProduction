using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeMeshSetter : MonoBehaviour
{
    [SerializeField] private TreeBehavior[] trees;

#if UNITY_EDITOR
    [ContextMenu("Get All trees")]
    public void GetAllTrees()
    {
        trees = GameObject.FindObjectsOfType<TreeBehavior>();
    }

    [ContextMenu("Set Mesh List")]
    public void SetAllTrees()
    {
        foreach(TreeBehavior t in trees)
        {
            t.GetAllMeshes();
        }
    }
#endif
}
