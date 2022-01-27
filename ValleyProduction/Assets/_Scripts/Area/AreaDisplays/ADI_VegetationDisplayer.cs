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

    [SerializeField] private List<VegetationDisplayData> vegetByValidScore;
    //[SerializeField] private List<InteractionSequence> possibleSequences;

    public override void OnUpdateScore(int newScore)
    {
        for (int i = 0; i < vegetByValidScore.Count; i++)
        {
            if (i < newScore)
            {
                for (int j = 0; j < vegetByValidScore[i].toDisplay.Count; j++)
                {
                    if (!vegetByValidScore[i].toDisplay[j].gameObject.activeSelf)
                    {
                        vegetByValidScore[i].toDisplay[j].SetTree();
                    }
                }
            }
            else
            {
                for (int j = 0; j < vegetByValidScore[i].toDisplay.Count; j++)
                {
                    if (vegetByValidScore[i].toDisplay[j].gameObject.activeSelf)
                    {
                        vegetByValidScore[i].toDisplay[j].UnsetTree();
                    }
                }
            }
        }
    }
}
