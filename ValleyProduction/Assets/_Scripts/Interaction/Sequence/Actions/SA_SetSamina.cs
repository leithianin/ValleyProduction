using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SA_SetSamina : InteractionActions
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
        CPN_Stamina staminaHandler = null;
        if(caller.HasComponent<CPN_Stamina>(ref staminaHandler))
        {
            staminaHandler.SetStaminaPercentage(staminaPercentageToPut);
        }
        EndAction(caller);
    }
}
