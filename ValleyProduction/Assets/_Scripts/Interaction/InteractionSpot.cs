using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionSpot : MonoBehaviour
{
    public InteractionType interactionType;

    [SerializeField] private UnityEvent PlayOnStartInteract;
    [SerializeField] private UnityEvent PlayOnEndInteract;

    public Action<InteractionHandler> PlayOnInteractionEnd;
    public Action<InteractionHandler> PlayOnInteractionStart;

    public InteractionActions interactionAction;

    /// <summary>
    /// Vérifie si l'interaction peut être faite.
    /// </summary>
    /// <returns>Renvoie TRUE si l'interaction peut être utilisé.</returns>
    public virtual bool IsUsable()
    {
        return true;
    }

    /// <summary>
    /// Fait intéragir l'InteractionHandler avec l'interaction actuelle.
    /// </summary>
    /// <param name="interacter">L'InteractionHandler qui demande à intéragir avec l'objet.</param>
    public void Interact(InteractionHandler interacter)
    {
        PlayOnStartInteract?.Invoke();
        PlayOnInteractionStart?.Invoke(interacter);
        interactionAction.PlayAction(interacter, () => EndInteraction(interacter));
    }

    /// <summary>
    /// Met fin à l'interaction.
    /// </summary>
    /// <param name="interacter">L'InteractionHandler qui finit son interaction.</param>
    public void EndInteraction(InteractionHandler interacter)
    {
        PlayOnEndInteract?.Invoke();
        PlayOnInteractionEnd?.Invoke(interacter);
    }
}
