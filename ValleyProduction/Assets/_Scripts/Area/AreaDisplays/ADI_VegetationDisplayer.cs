using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ADI_VegetationDisplayer : EcosystemDisplay
{
    [Serializable]
    private class VegetationDisplayData
    {
        public List<TreeBehavior> toDisplay;
    }

    [SerializeField] private float scoreByTree;
    private float currentTreeScore = 0;

    [SerializeField] private List<TreeBehavior> treesInArea = new List<TreeBehavior>();

    [SerializeField] private List<int> numberTreeToDisabled;

    [SerializeField] private UnityEvent<float> OnChangeScore;

    [ContextMenu("Set Trees")]
    public void SetTrees()
    {
        treesInArea = new List<TreeBehavior>();

        Collider[] colliderInArea = Physics.OverlapBox(transform.position, new Vector3(1,3,1) * 12.5f);

        for(int i = 0; i < colliderInArea.Length; i++)
        {
            if(colliderInArea[i].GetComponent<TreeBehavior>() != null)
            {
                treesInArea.Add(colliderInArea[i].GetComponent<TreeBehavior>());
            }
        }

        if(treesInArea.Count == 0)
        {
            DestroyImmediate(gameObject);
        }
    }

    protected override void OnStart()
    {
        //OnUpdateScore(-1);

        currentTreeScore = treesInArea.Count * scoreByTree;
        OnChangeScore?.Invoke(currentTreeScore);
    }

    public override void OnUpdateScore(int newScore)
    {
        int currentTreeIndex = -1;

        for(int i = 0; i < numberTreeToDisabled.Count; i++)
        {
            for(int j = 0; j < numberTreeToDisabled[i]; j++)
            {
                currentTreeIndex++;
                if (currentTreeIndex < treesInArea.Count)
                {
                    if (i < newScore)
                    {
                        //Debug.Log(currentTreeIndex + " New score");
                        if (treesInArea[currentTreeIndex].IsSet)
                        {
                            //Debug.Log(currentTreeIndex + "IsSet");
                            treesInArea[currentTreeIndex].UnsetTree();
                            currentTreeScore -= scoreByTree;
                        }
                    }
                    else
                    {
                        //Debug.Log(currentTreeIndex + " NOT New Score");
                        if (!treesInArea[currentTreeIndex].IsSet)
                        {
                            //Debug.Log(currentTreeIndex + "NOT IsSet");
                            treesInArea[currentTreeIndex].SetTree();
                            currentTreeScore += scoreByTree;
                        }
                    }
                }
            }
        }

        OnChangeScore?.Invoke(currentTreeScore);
    }
}
