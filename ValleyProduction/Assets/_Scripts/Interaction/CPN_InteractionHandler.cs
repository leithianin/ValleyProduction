using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CPN_InteractionHandler : VLY_Component<CPN_Data_Interaction>
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

    public override void SetData(CPN_Data_Interaction dataToSet)
    {
        interactionTypes = new List<InteractionType>(dataToSet.InteractionInterest());
    }
}
