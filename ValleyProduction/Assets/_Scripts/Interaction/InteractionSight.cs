using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractionSight : MonoBehaviour
{
    [SerializeField] protected CPN_InteractionHandler interactor;

    protected bool isInteracting;

    [SerializeField] private InteractionSpot currentSpot;
    [SerializeField] private float timeBetweenInteractions = 5f;

    public bool IsInteracting => isInteracting;

    public CPN_InteractionHandler Interactor => interactor;

    private void OnTriggerEnter(Collider other)
    {
        if (!isInteracting)
        {
            InteractionSpot spot = other.GetComponent<InteractionSpot>();

            if (spot != null && spot.IsUsable(interactor))
            {
                StartInteraction(spot);
            }
        }
    }

    /// <summary>
    /// Actions spécifiques à faire lors du commencement d'une interaction.
    /// </summary>
    /// <param name="spot">L'objet avec lequel il interagit.</param>
    public abstract void OnStartInteraction(InteractionSpot spot);

    /// <summary>
    /// Actions spécifiques à faire lors de la fin d'une interaction.
    /// </summary>
    public abstract void OnEndInteraction();

    /// <summary>
    /// Appelé quand on demande à interagir avec un Spot.
    /// </summary>
    /// <param name="spot">The spot the interactor want to interact with.</param>
    public void StartInteraction(InteractionSpot spot)
    {
        if (interactor.IsInterested(spot.interactionType))
        {
            currentSpot = spot;
            OnStartInteraction(spot);
            isInteracting = true;
            spot.PlayOnInteractionEnd += EndInteraction;

            spot.Interact(interactor);
        }
    }

    /// <summary>
    /// Appelé quand l'interaction avec le Spot se finit.
    /// </summary>
    /// <param name="spotInteractor">L'InteractionHandler qui vient de finir l'interaction.</param>
    private void EndInteraction(CPN_InteractionHandler spotInteractor)
    {
        if(spotInteractor == interactor)
        {
            currentSpot.PlayOnInteractionEnd -= EndInteraction;
            TimerManager.CreateGameTimer(timeBetweenInteractions, () => isInteracting = false);
            isInteracting = false;
            OnEndInteraction();
        }
    }
}
