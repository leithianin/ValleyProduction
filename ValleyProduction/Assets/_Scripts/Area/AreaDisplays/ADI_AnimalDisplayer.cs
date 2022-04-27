using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADI_AnimalDisplayer : EcosystemDisplay
{
    public int score = 0;

    [SerializeField] private int currentAnimalCount;

    [Serializable]
    private class AnimalDisplayData
    {
        public List<AnimalBehavior> toDisplay;
    }

    [SerializeField] private List<AnimalDisplayData> animalsByValidScore;
    [SerializeField] private List<InteractionSequence> possibleSequences;

    public int AnimalCount => currentAnimalCount;

    public override void OnUpdateScore(int newScore)
    {
        currentAnimalCount = 0;

        score = newScore;
        for(int i = 0; i < animalsByValidScore.Count; i++)
        {
            if (animalsByValidScore[i].toDisplay[0] != null)
            {
                if (i < newScore)
                {
                    for (int j = 0; j < animalsByValidScore[i].toDisplay.Count; j++)
                    {
                        currentAnimalCount++;
                        if (!animalsByValidScore[i].toDisplay[j].display.activeSelf)
                        {
                            animalsByValidScore[i].toDisplay[j].SetAnimal(possibleSequences[UnityEngine.Random.Range(0, possibleSequences.Count)]);
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < animalsByValidScore[i].toDisplay.Count; j++)
                    {
                        if (animalsByValidScore[i].toDisplay[j].display.activeSelf)
                        {
                            animalsByValidScore[i].toDisplay[j].UnsetAnimal();
                        }
                    }
                }
            }
        }
    }
}
