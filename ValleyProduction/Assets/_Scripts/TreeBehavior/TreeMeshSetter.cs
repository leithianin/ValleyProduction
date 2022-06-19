using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TreeMeshSetter : MonoBehaviour
{
    [SerializeField] private TreeBehavior[] trees;

    [SerializeField] private List<Transform> parrentToActivate;

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

    [ContextMenu("Set all objects")]
    public void EnableAllObjects()
    {
        foreach(Transform tr in parrentToActivate)
        {
            ActivateAllChildren(tr);
        }
    }

    private void ActivateAllChildren(Transform parent)
    {
        foreach(Transform tr in parent)
        {
            tr.gameObject.SetActive(true);
            if(tr.childCount > 0)
            {
                ActivateAllChildren(tr);
            }
            EditorUtility.SetDirty(tr.gameObject);
        }
    }
#endif
}
