using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterestPointDetectionSight : MonoBehaviour
{
    [SerializeField] private InteractionSight interactionSight;

    private void OnTriggerEnter(Collider other)
    {
        if (!interactionSight.IsInteracting)
        {
            InterestPoint interaction = other.GetComponent<InterestPoint>();

            if (interaction != null && interaction.IsUsable(interactionSight.Interactor))
            {
                interactionSight.StartInteraction(interaction.GetRandomSpot(interactionSight.Interactor));
            }
        }
    }

    //Faire une fonction pour v�rifier si c'est un InterestPoint voulut par le visiteur
}
