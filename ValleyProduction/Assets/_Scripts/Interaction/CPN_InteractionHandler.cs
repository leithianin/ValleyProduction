using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CPN_InteractionHandler : VLY_Component
{
    [SerializeField] private VLY_ComponentHandler handler;

    [SerializeField] private List<InteractionType> interactionTypes;

    public VLY_ComponentHandler Handler => handler;

    public bool HasComponent<T>(ref T wantedComponent) where T : VLY_Component
    {
        T inHandler = null;
        handler.GetComponentOfType<T>(ref inHandler);

        wantedComponent = inHandler;

        return wantedComponent != null;
    }

    public bool IsInterested(InteractionType wantedType)
    {
        return interactionTypes.Contains(wantedType);
    }

    public void SetInteractableData(IInteractableData newDatas)
    {
        interactionTypes = newDatas.GetInteractionTypes();
    }
}
