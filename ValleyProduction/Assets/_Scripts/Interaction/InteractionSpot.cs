using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionSpot : MonoBehaviour
{
    public BuildTypes interactionType;
    public float attractivityLevel;

    [SerializeField] private List<InteractionCondition> conditions;

    [SerializeField] private int maxInteractionAtSameTime = -1;

    [SerializeField] private UnityEvent PlayOnStartInteract;
    [SerializeField] private UnityEvent PlayOnEndInteract;
    [SerializeField] private UnityEvent PlayOnInteruptInteract;

    public Action<CPN_InteractionHandler> PlayOnInteractionEnd;
    public Action<CPN_InteractionHandler> PlayOnInteractionStart;
    public Action<CPN_InteractionHandler> PlayOnInteractionInterupt;

    public InteractionActions interactionAction;

    [SerializeField] private List<CPN_InteractionHandler> callerInSpot = new List<CPN_InteractionHandler>();

    /// <summary>
    /// V�rifie si l'interaction peut �tre faite.
    /// </summary>
    /// <returns>Renvoie TRUE si l'interaction peut �tre utilis�.</returns>
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
    /// Fait int�ragir l'InteractionHandler avec l'interaction actuelle.
    /// </summary>
    /// <param name="interacter">L'InteractionHandler qui demande � int�ragir avec l'objet.</param>
    public void Interact(CPN_InteractionHandler interacter)
    {
        //Debug.Log("Interaction : " + interacter.name);
        callerInSpot.Add(interacter);

        PlayOnStartInteract?.Invoke();
        PlayOnInteractionStart?.Invoke(interacter);
        if (interactionAction != null)
        {
            interactionAction.PlayAction(interacter, () => EndInteraction(interacter), () => EndInteraction(interacter));
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
    public void EndInteraction(CPN_InteractionHandler interacter)
    {
        callerInSpot.Remove(interacter);

        PlayOnEndInteract?.Invoke();
        PlayOnInteractionEnd?.Invoke(interacter);
    }

    public void AskToInterupt()
    {
        while(callerInSpot.Count > 0)
        { 
            interactionAction.InteruptAction(callerInSpot[0]);
        }
    }
}
