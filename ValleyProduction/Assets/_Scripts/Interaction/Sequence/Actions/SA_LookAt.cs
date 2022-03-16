using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SA_LookAt : InteractionActions
{
    [SerializeField] private Transform lookTarget;

    protected override void OnPlayAction(CPN_InteractionHandler caller)
    {
        caller.transform.forward = lookTarget.position - caller.transform.position;

        caller.transform.eulerAngles = new Vector3(0, caller.transform.eulerAngles.y, 0);

        EndAction(caller);
    }

    protected override void OnEndAction(CPN_InteractionHandler caller)
    {

    }

    protected override void OnInteruptAction(CPN_InteractionHandler caller)
    {
        
    }
}
