using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionHandler : MonoBehaviour
{
    [SerializeField] private CPN_Movement movement;

    [SerializeField] private List<InteractionType> interactionTypes;

    public CPN_Movement Movement => movement;

    public bool IsInterested(InteractionType wantedType)
    {
        return interactionTypes.Contains(wantedType);
    }

    public void SetInteractableData(IInteractableData newDatas)
    {
        interactionTypes = newDatas.GetInteractionTypes();
    }
}
