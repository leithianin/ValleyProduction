using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InterestPoint : MonoBehaviour
{
    [SerializeField] private InteractionSpot[] interactions;

    public Action<InterestPoint> OnDisableInterestPoint;

    public void DisableInterestPoint()
    {
        OnDisableInterestPoint?.Invoke(this);
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

    public List<BuildTypes> InteractionTypeInInterestPoint()
    {
        List<BuildTypes> toReturn = new List<BuildTypes>();

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

    public float GetAttractivityScore(List<BuildTypes> likedTypes, List<BuildTypes> hatedTypes)
    {
        float toReturn = 0;

        for(int i = 0; i < interactions.Length; i++)
        {
            if(likedTypes.Contains(interactions[i].interactionType))
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
}
