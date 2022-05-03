using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionSpot : MonoBehaviour
{
    public SatisfactorType interactionType;
    public float attractivityLevel;

    [SerializeField] private List<InteractionCondition> conditions;

    [SerializeField] public int maxInteractionAtSameTime = -1;

    [SerializeField] private UnityEvent<CPN_InteractionHandler> PlayOnStartInteract;
    [SerializeField] private UnityEvent<CPN_InteractionHandler> PlayOnEndInteract;
    [SerializeField] private UnityEvent<CPN_InteractionHandler> PlayOnInteruptInteract;
    [SerializeField] private UnityEvent PlayOnAddVisitors;
    [SerializeField] private UnityEvent PlayOnRemoveVisitors;

    public Action<CPN_InteractionHandler> PlayOnInteractionEnd;
    public Action<CPN_InteractionHandler> PlayOnInteractionStart;
    public Action<CPN_InteractionHandler> PlayOnInteractionInterupt;

    public InteractionActions interactionAction;

    [SerializeField] private List<CPN_InteractionHandler> callerInSpot = new List<CPN_InteractionHandler>();

    public int currentNbVisitors = 0;

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
        //Debug.Log("Interaction : " + interacter.name);
        callerInSpot.Add(interacter);

        PlayOnStartInteract?.Invoke(interacter);
        PlayOnInteractionStart?.Invoke(interacter);
        if (interactionAction != null)
        {
            AddVisitors();
            interactionAction.PlayAction(interacter, () => EndInteraction(interacter), () => EndInteraction(interacter), false);
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
        currentNbVisitors--;
        RemoveVisitors();
        callerInSpot.Remove(interacter);
        PlayOnEndInteract?.Invoke(interacter);
        PlayOnInteractionEnd?.Invoke(interacter);
    }

    public void AskToInterupt()
    {
        while(callerInSpot.Count > 0)
        { 
            interactionAction.InteruptAction(callerInSpot[0]);
        }
    }

    /// <summary>
    /// Add 1 visitors to data. Event go to Infrastructure -> AddTotalVisitors()
    /// </summary>
    public void AddVisitors()
    {
        currentNbVisitors++;
        PlayOnAddVisitors?.Invoke();
    }

    /// <summary>
    /// Update current Nb because we remove 1
    /// </summary>
    public void RemoveVisitors()
    {
        UIManager.UpdateCurrentNbVisitors();
    }
}
