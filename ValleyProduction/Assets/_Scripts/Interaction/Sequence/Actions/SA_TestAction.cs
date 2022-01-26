using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SA_TestAction : InteractionActions
{
    protected override void OnEndAction(InteractionHandler caller)
    {
        
    }

    protected override void OnInteruptAction(InteractionHandler caller)
    {
        StopCoroutine(WaitForNextAction(caller));
    }

    protected override void OnPlayAction(InteractionHandler caller)
    {
        StartCoroutine(WaitForNextAction(caller));
    }

    IEnumerator WaitForNextAction(InteractionHandler caller)
    {
        yield return new WaitForSeconds(.5f);
        EndAction(caller);
    }
}
