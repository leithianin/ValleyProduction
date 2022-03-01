using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAnimals : MonoBehaviour
{
    public InteractionSequence interacSequence;
    public List<AnimalBehavior> animalBehaviorList = new List<AnimalBehavior>();

    public void SetAnimal()
    {
        foreach(AnimalBehavior animal in animalBehaviorList)
        {
            animal.SetAnimal(interacSequence);
        }
    }
}
