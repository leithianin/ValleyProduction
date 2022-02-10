using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SA_TestAction : InteractionActions
{
    protected override void OnEndAction(CPN_InteractionHandler caller)
    {
        
    }

    protected override void OnInteruptAction(CPN_InteractionHandler caller)
    {
        StopCoroutine(WaitForNextAction(caller));
    }

    protected override void OnPlayAction(CPN_InteractionHandler caller)
    {
        StartCoroutine(WaitForNextAction(caller));
    }

    IEnumerator WaitForNextAction(CPN_InteractionHandler caller)
    {
        yield return new WaitForSeconds(.5f);
        EndAction(caller);
    }
}
