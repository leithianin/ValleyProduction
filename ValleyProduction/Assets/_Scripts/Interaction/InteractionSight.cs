using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractionSight : MonoBehaviour
{
    [SerializeField] protected InteractionHandler interactor;

    protected bool isInteracting;

    private InteractionSpot currentSpot;

    public bool IsInteracting => isInteracting;

    private void OnTriggerEnter(Collider other)
    {
        if (!isInteracting)
        {
            InteractionSpot spot = other.GetComponent<InteractionSpot>();

            if (spot != null && spot.IsUsable())
            {
                StartInteraction(spot);
            }
        }
    }

    public abstract void OnStartInteraction(InteractionSpot spot);

    public abstract void OnEndInteraction(InteractionHandler spotInteractor);

    public void StartInteraction(InteractionSpot spot)
    {
        currentSpot = spot;
        OnStartInteraction(spot);
        isInteracting = true;
        spot.PlayOnInteractionEnd += EndInteraction;

        spot.Interact(interactor);
    }

    private void EndInteraction(InteractionHandler spotInteractor)
    {
        if(spotInteractor == interactor)
        {
            currentSpot.PlayOnInteractionEnd -= EndInteraction;
            isInteracting = false;
            OnEndInteraction(spotInteractor);
        }
    }
}
