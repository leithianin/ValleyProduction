using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionTest : VisitorInteraction
{
    public override bool IsUsable()
    {
        return true;
    }

    public override void OnInteractionEnd(VisitorBehavior visitor)
    {
        
    }

    public override void OnVisitorInteract(VisitorBehavior visitor)
    {
        StartCoroutine(TestInteraction(visitor));
    }

    IEnumerator TestInteraction(VisitorBehavior visitor)
    {
        yield return new WaitForSeconds(3f);
        EndInteraction(visitor);
    }
}
