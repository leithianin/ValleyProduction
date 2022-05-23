using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ECO_DIS_VegetationDisplayer : EcosystemDisplay
{
    [SerializeField] private List<TreeBehavior> trees;

    public int score;

    public override void OnUpdateScore(int newScore)
    {
        score = newScore;

        for(int i = 0; i < trees.Count; i++)
        {
            if(newScore != trees[i].CurrentPhase)
            {
                trees[i].SetTreePhase(newScore);
            }

            trees[i].SetTreePhase(newScore);
            /*if(newScore > 0 && trees[i].IsSet)
            {
                trees[i].UnsetTree();
            }
            else if (newScore <= 0 && !trees[i].IsSet)
            {
                trees[i].SetTree();
            }*/
        }
    }
}
