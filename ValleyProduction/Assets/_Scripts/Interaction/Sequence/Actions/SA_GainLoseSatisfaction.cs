using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SA_GainLoseSatisfaction : InteractionActions
{
    [SerializeField] private float amountToAdd;

    protected override void OnPlayAction(CPN_InteractionHandler caller)
    {
        if(caller.HasComponent<CPN_SatisfactionHandler>(out CPN_SatisfactionHandler satisfactionHandler))
        {
            satisfactionHandler.AddSatisfaction(amountToAdd);
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
