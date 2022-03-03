using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SA_SetStamina : InteractionActions
{
    [SerializeField] private float staminaPercentageToPut;

    protected override void OnEndAction(CPN_InteractionHandler caller)
    {
        
    }

    protected override void OnInteruptAction(CPN_InteractionHandler caller)
    {
        
    }

    protected override void OnPlayAction(CPN_InteractionHandler caller)
    {
        if(caller.HasComponent<CPN_Stamina>(out CPN_Stamina staminaHandler))
        {
            staminaHandler.SetStaminaPercentage(staminaPercentageToPut);
        }
        EndAction(caller);
    }
}
