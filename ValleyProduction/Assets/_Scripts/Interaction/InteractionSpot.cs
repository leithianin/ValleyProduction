using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionSpot : MonoBehaviour
{
    public InteractionType interactionType;

    [SerializeField] private List<InteractionCondition> conditions;

    [SerializeField] private int maxInteractionAtSameTime = -1;

    [SerializeField] private UnityEvent PlayOnStartInteract;
    [SerializeField] private UnityEvent PlayOnEndInteract;

    public Action<CPN_InteractionHandler> PlayOnInteractionEnd;
    public Action<CPN_InteractionHandler> PlayOnInteractionStart;

    public InteractionActions interactionAction;

    private List<CPN_InteractionHandler> callerInSpot = new List<CPN_InteractionHandler>();

    /// <summary>
    /// Vérifie si l'interaction peut être faite.
    /// </summary>
    /// <returns>Renvoie TRUE si l'interaction peut être utilisé.</returns>
    public virtual bool IsUsable(CPN_InteractionHandler interacter)
    {
        bool isConditionTrue = maxInteractionAtSameTime > 0 && callerInSpot.Count < maxInteractionAtSameTime;

        if (isConditionTrue == true)
        {
            for (int i = 0; i < conditions.Count; i++)
            {
                if (!conditions[i].IsConditionTrue(interacter))
                {
                    isConditionTrue = false;
                    break;
                }
            }
        }

        return isConditionTrue;
    }

    /// <summary>
    /// Fait intéragir l'InteractionHandler avec l'interaction actuelle.
    /// </summary>
    /// <param name="interacter">L'InteractionHandler qui demande à intéragir avec l'objet.</param>
    public void Interact(CPN_InteractionHandler interacter)
    {
        Debug.Log("Interaction");
        callerInSpot.Add(interacter);

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
    /// Met fin à l'interaction.
    /// </summary>
    /// <param name="interacter">L'InteractionHandler qui finit son interaction.</param>
    public void EndInteraction(CPN_InteractionHandler interacter)
    {
        callerInSpot.Remove(interacter);

        PlayOnEndInteract?.Invoke();
        PlayOnInteractionEnd?.Invoke(interacter);
    }
}
