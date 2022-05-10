using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalDetector : MonoBehaviour
{
    [SerializeField] private List<AnimalBehavior> animalsInZone = new List<AnimalBehavior>();

    private void OnTriggerEnter(Collider other)
    {
        AnimalBehavior behaviors = other.GetComponent<AnimalBehavior>();
        if(behaviors != null && !animalsInZone.Contains(behaviors))
        {
            animalsInZone.Add(behaviors);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        AnimalBehavior behaviors = other.GetComponent<AnimalBehavior>();

        if (behaviors != null && animalsInZone.Contains(behaviors))
        {
            animalsInZone.Remove(behaviors);
        }
    }

    public float GetAnimalAttractivityScore()
    {
        float toReturn = 0;

        for(int i = 0; i < animalsInZone.Count; i++)
        {
            if(animalsInZone[i].Interactor.HasComponent<CPN_SatisfactionGiver>(out CPN_SatisfactionGiver satisfaction))
            {
                toReturn += satisfaction.SatisfactionGiven;
            }
        }

        return toReturn;
    }

    public void SeeAllAnimals(CPN_InteractionHandler interactor)
    {
        if(interactor.HasComponent<CPN_SatisfactionHandler>(out CPN_SatisfactionHandler satisfaction))
        {
            satisfaction.AddSatisfaction(GetAnimalAttractivityScore());
        }
    }
}
