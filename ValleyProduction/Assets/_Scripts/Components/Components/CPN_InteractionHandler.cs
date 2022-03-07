using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CPN_InteractionHandler : VLY_Component<CPN_Data_Interaction>
{
    [SerializeField] private VLY_ComponentHandler handler;

    [SerializeField] private List<BuildsTypes> interactionTypes;

    public VLY_ComponentHandler Handler => handler;

    public bool HasComponent<T>(out T wantedComponent) where T : VLY_Component
    {
        T inHandler = handler.GetComponentOfType<T>();

        wantedComponent = inHandler;

        return wantedComponent != null;
    }

    public bool IsInterested(BuildsTypes wantedType)
    {
        return interactionTypes.Contains(wantedType);
    }

    public override void SetData(CPN_Data_Interaction dataToSet)
    {
        interactionTypes = new List<BuildsTypes>(dataToSet.InteractionInterest());
    }
}
