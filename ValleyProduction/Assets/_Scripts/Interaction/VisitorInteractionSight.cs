using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorInteractionSight : MonoBehaviour
{
    [SerializeField] private VisitorBehavior visitor;

    private bool isInteracting;

    private void OnTriggerEnter(Collider other)
    {
        if (!isInteracting)
        {
            VisitorInteraction interaction = other.GetComponent<VisitorInteraction>();

            if (interaction != null && interaction.IsUsable())
            {
                interaction.Interact(visitor);
                isInteracting = true;
                visitor.StopWalk();
                interaction.PlayOnInteractionEnd += EndInteraction;
            }
        }
    }

    private void EndInteraction(VisitorBehavior interactionVisitor)
    {
        if(interactionVisitor == visitor)
        {
            visitor.ContinueWalk();
            isInteracting = false;
        }
    }
}
