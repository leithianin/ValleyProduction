using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VisitorInteraction : MonoBehaviour
{
    public Action<VisitorBehavior> PlayOnInteractionEnd;
    public Action<VisitorBehavior> PlayOnInteractionStart;

    public abstract bool IsUsable();

    public abstract void OnVisitorInteract(VisitorBehavior visitor);

    public abstract void OnInteractionEnd(VisitorBehavior visitor);

    public void Interact(VisitorBehavior visitor)
    {
        visitor.StopWalk();
        PlayOnInteractionStart?.Invoke(visitor);
        OnVisitorInteract(visitor);
    }

    public void EndInteraction(VisitorBehavior visitor)
    {
        OnInteractionEnd(visitor);
        PlayOnInteractionEnd?.Invoke(visitor);
        visitor.ContinueWalk();
    }
}
