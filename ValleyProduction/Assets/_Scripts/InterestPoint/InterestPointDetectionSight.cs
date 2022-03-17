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
                InteractionSpot spot = interaction.GetRandomSpot(interactionSight.Interactor);

                if (spot != null)
                {
                    interactionSight.StartInteraction(spot);
                }
            }
        }
    }
}
