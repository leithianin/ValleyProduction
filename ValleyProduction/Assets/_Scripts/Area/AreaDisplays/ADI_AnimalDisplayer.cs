using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADI_AnimalDisplayer : AreaDisplay
{
    [Serializable]
    private class AnimalDisplayData
    {
        public List<AnimalBehavior> toDisplay;
    }

    [SerializeField] private List<AnimalDisplayData> animalsByValidScore;
    [SerializeField] private List<InteractionSequence> possibleSequences;

    public override void OnUpdateScore(int newScore)
    {
        for(int i = 0; i < animalsByValidScore.Count; i++)
        {
            if(i < newScore)
            {
                for(int j = 0; j < animalsByValidScore[i].toDisplay.Count; j++)
                {
                    if (!animalsByValidScore[i].toDisplay[j].gameObject.activeSelf)
                    {
                        animalsByValidScore[i].toDisplay[j].SetAnimal(possibleSequences[UnityEngine.Random.Range(0, possibleSequences.Count)]);
                    }
                }
            }
            else
            {
                for (int j = 0; j < animalsByValidScore[i].toDisplay.Count; j++)
                {
                    if (animalsByValidScore[i].toDisplay[j].gameObject.activeSelf)
                    {
                        animalsByValidScore[i].toDisplay[j].UnsetAnimal();
                    }
                }
            }
        }
    }
}
