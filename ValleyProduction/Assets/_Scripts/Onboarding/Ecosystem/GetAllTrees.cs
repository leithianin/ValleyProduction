using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete]
public class GetAllTrees : MonoBehaviour
{
    [SerializeField]private List<TreeBehavior> treeList = new List<TreeBehavior>();
    public GameObject treeParent;

    [ContextMenu ("GetAllTrees")]
    public void GetAllTreesFunc()
    {
        treeList.Clear();
        for(int i = 0; i < treeParent.transform.childCount; i++)
        {
            if(treeParent.transform.GetChild(i).GetComponent<TreeBehavior>())
            {
                treeList.Add(treeParent.transform.GetChild(i).GetComponent<TreeBehavior>());
            }
        }

        foreach(TreeBehavior treeBehaviour in treeList)
        {
           //treeBehaviour.UnsetTree();
        }

        Debug.Log(treeList.Count);
    }

    public void SetAllTreesToHealthly()
    {
        foreach (TreeBehavior treeBehaviour in treeList)
        {
            //treeBehaviour.SetTree();
        }
    }
}
