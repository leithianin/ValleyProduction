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
    /// V�rifie si l'interaction peut �tre faite.
    /// </summary>
    /// <returns>Renvoie TRUE si l'interaction peut �tre utilis�.</returns>
    public virtual bool IsUsable()
    {
        return true;
    }

    /// <summary>
    /// Fait int�ragir l'InteractionHandler avec l'interaction actuelle.
    /// </summary>
    /// <param name="interacter">L'InteractionHandler qui demande � int�ragir avec l'objet.</param>
    public void Interact(InteractionHandler interacter)
    {
        Debug.Log("Interaction");
        PlayOnStartInteract?.Invoke();
        PlayOnInteractionStart?.Invoke(interacter);
        if (interactionAction != null)
        {
            interactionAction.PlayAction(interacter, () => EndInteraction(interacter));
        }
        else
        {
            EndInteraction(interacter);
        }
    }

    /// <summary>
    /// Met fin � l'interaction.
    /// </summary>
    /// <param name="interacter">L'InteractionHandler qui finit son interaction.</param>
    public void EndInteraction(InteractionHandler interacter)
    {
        PlayOnEndInteract?.Invoke();
        PlayOnInteractionEnd?.Invoke(interacter);
    }
}
