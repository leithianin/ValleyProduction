using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADI_VegetationDisplayer : AreaDisplay
{
    [Serializable]
    private class VegetationDisplayData
    {
        public List<TreeBehavior> toDisplay;
    }

    //[SerializeField] private List<VegetationDisplayData> vegetByValidScore;
    [SerializeField] private List<TreeBehavior> treesInArea;

    [SerializeField] private List<int> treeToDisable;

    //[SerializeField] private List<InteractionSequence> possibleSequences;

    [ContextMenu("Set Trees")]
    private void SetTrees()
    {
        treesInArea = new List<TreeBehavior>();

        Collider[] colliderInArea = Physics.OverlapBox(transform.position, Vector3.one * 12.5f);

        for(int i = 0; i < colliderInArea.Length; i++)
        {
            if(colliderInArea[i].GetComponent<TreeBehavior>() != null)
            {
                treesInArea.Add(colliderInArea[i].GetComponent<TreeBehavior>());
            }
        }
    }

    public int score;

    public override void OnUpdateScore(int newScore)
    {
        score = newScore;
        int currentTreeIndex = 0;

        for(int i = 0; i < treeToDisable.Count; i++)
        {
            if (i < newScore)
            {
                if (!treesInArea[currentTreeIndex].IsSet)
                {
                    treesInArea[currentTreeIndex].SetTree();
                }
                currentTreeIndex++;
            }
            else
            {
                if (treesInArea[currentTreeIndex].IsSet)
                {
                    treesInArea[currentTreeIndex].UnsetTree();
                }
                currentTreeIndex++;
            }
        }

        /*for (int i = 0; i < vegetByValidScore.Count; i++)
        {
            if (i < newScore)
            {
                for (int j = 0; j < vegetByValidScore[i].toDisplay.Count; j++)
                {
                    if (!vegetByValidScore[i].toDisplay[j].IsSet)
                    {
                        vegetByValidScore[i].toDisplay[j].SetTree();
                    }
                }
            }
            else
            {
                for (int j = 0; j < vegetByValidScore[i].toDisplay.Count; j++)
                {
                    if (vegetByValidScore[i].toDisplay[j].IsSet)
                    {
                        vegetByValidScore[i].toDisplay[j].UnsetTree();
                    }
                }
            }
        }*/
    }
}
