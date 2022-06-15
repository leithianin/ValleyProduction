using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InterestPoint : MonoBehaviour
{
    [SerializeField] public InteractionSpot[] interactions;

    public Action<InterestPoint> OnDisableInterestPoint;

    public void EnableInterestPoint()
    {
        gameObject.SetActive(true);
    }

    public void DisableInterestPoint()
    {
        OnDisableInterestPoint?.Invoke(this);

        AskToInterupt();

        gameObject.SetActive(false);
    }

    public bool IsUsable(CPN_InteractionHandler interactor)
    {
        return GetUsableInteractions(interactor).Count > 0;
    }

    public void AskToInterupt()
    {
        for(int i = 0; i < interactions.Length; i++)
        {
            interactions[i].AskToInterupt();
        }
    }

    public List<SatisfactorType> InteractionTypeInInterestPoint()
    {
        List<SatisfactorType> toReturn = new List<SatisfactorType>();

        for (int i = 0; i < interactions.Length; i++)
        {
            if(!toReturn.Contains(interactions[i].interactionType))
            {
                toReturn.Add(interactions[i].interactionType);
            }
        }
        return toReturn;
    }

    public InteractionSpot GetRandomSpot(CPN_InteractionHandler interactor)
    {
        List<InteractionSpot> usableInteractions = GetUsableInteractions(interactor);

        if (usableInteractions.Count > 0)
        {
            return usableInteractions[UnityEngine.Random.Range(0, usableInteractions.Count)];
        }
        return null;
    }

    private List<InteractionSpot> GetUsableInteractions(CPN_InteractionHandler interactor)
    {
        List<InteractionSpot> usableInteractions = new List<InteractionSpot>();

        for(int i = 0; i < interactions.Length; i++)
        {
            if(interactions[i].IsUsable(interactor))
            {
                usableInteractions.Add(interactions[i]);
            }
        }

        return usableInteractions;
    }

    public float GetAttractivityScore(List<SatisfactorType> likedTypes, List<SatisfactorType> hatedTypes)
    {
        float toReturn = 0;

        for(int i = 0; i < interactions.Length; i++)
        {
            if((likedTypes.Count > 0 && interactions[i].interactionType == SatisfactorType.All) || likedTypes.Contains(interactions[i].interactionType))
            {
                toReturn += interactions[i].attractivityLevel;
            }
            else if(hatedTypes.Contains(interactions[i].interactionType))
            {
                toReturn -= interactions[i].attractivityLevel;
            }
        }

        return toReturn;
    }

    public int GetInteractionMaxVisitors()
    {
        int nb = 0;

        foreach(InteractionSpot interacSpot in interactions)
        {
            nb += interacSpot.maxInteractionAtSameTime;
        }

        return nb;
    }

    public int GetCurrentNbVisitors()
    {
        int nb = 0;

        foreach (InteractionSpot interacSpot in interactions)
        {
            nb += interacSpot.currentNbVisitors;
        }

        return nb;
    }
}
