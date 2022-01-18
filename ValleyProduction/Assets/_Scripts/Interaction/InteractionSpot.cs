using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSpot : MonoBehaviour
{
    public Action<InteractionHandler> PlayOnInteractionEnd;
    public Action<InteractionHandler> PlayOnInteractionStart;

    public InteractionActions interactionAction;

    public virtual bool IsUsable()
    {
        return true;
    }

    public void Interact(InteractionHandler interacter)
    {
        PlayOnInteractionStart?.Invoke(interacter);
        interactionAction.PlayAction(interacter, () => EndInteraction(interacter));
    }

    public void EndInteraction(InteractionHandler interacter)
    {
        PlayOnInteractionEnd?.Invoke(interacter);
    }
}
