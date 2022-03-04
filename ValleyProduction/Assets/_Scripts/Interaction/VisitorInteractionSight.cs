using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorInteractionSight : InteractionSight
{
    [SerializeField] private VisitorBehavior visitor;

    public override void OnEndInteraction()
    {
        Debug.Log("End Interaction");
        visitor.ContinueWalk();
    }

    public override void OnStartInteraction(InteractionSpot spot)
    {
        visitor.InteruptWalk();
    }
}
