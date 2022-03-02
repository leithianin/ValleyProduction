using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InterestPoint : MonoBehaviour
{
    [SerializeField] private bool isLandmark;

    [SerializeField] private InteractionSpot[] interactions;

    public bool IsUsable(CPN_InteractionHandler interactor)
    {
        return GetUsableInteractions(interactor).Count > 0;
    }

    public bool IsLandmark => isLandmark;

    public InteractionSpot GetRandomSpot(CPN_InteractionHandler interactor)
    {
        List<InteractionSpot> usableInteractions = GetUsableInteractions(interactor);

        if (usableInteractions.Count > 0)
        {
            return interactions[Random.Range(0, usableInteractions.Count)];
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
}
