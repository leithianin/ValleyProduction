using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SA_ChangePollutionThrowDelay : InteractionActions
{
    [SerializeField] private float addThrowTime;

    protected override void OnPlayAction(CPN_InteractionHandler caller)
    {
        if (caller.HasComponent<CPN_TrashThrower>(out CPN_TrashThrower trashThrower))
        {
            trashThrower.DelayThrow(addThrowTime);
        }

        EndAction(caller);
    }

    protected override void OnEndAction(CPN_InteractionHandler caller)
    {
        
    }

    protected override void OnInteruptAction(CPN_InteractionHandler caller)
    {
        
    }
}
