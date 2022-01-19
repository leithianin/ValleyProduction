using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SA_Wait : InteractionActions
{
    [SerializeField] private Vector2 timeToWait;
    protected override void OnEndAction(InteractionHandler caller)
    {
        
    }

    protected override void OnPlayAction(InteractionHandler caller)
    {
        StartCoroutine(WaitForNextAction(caller));
    }

    IEnumerator WaitForNextAction(InteractionHandler caller)
    {
        float toWait = Random.Range(timeToWait.x, timeToWait.y);
        yield return new WaitForSeconds(toWait);
        EndAction(caller);
    }
}
