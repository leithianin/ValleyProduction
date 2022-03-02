using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IC_CheckInteractorStat_Stamina : InteractionCondition
{
    [SerializeField, Tooltip("If true, will check if the stamina is over the limit. If false, will check if the stamina is under the limit.")] private bool isStaminaOver;
    [SerializeField, Tooltip("If less than 0, will not be check.")] private float staminaLimit;

    public override bool IsConditionTrue(CPN_InteractionHandler interacter)
    {
        bool toReturn = true;

        CPN_Stamina stamina = null;

        if (interacter.HasComponent<CPN_Stamina>(ref stamina))
        {
            if (staminaLimit >= 0)
            {
                if(isStaminaOver != stamina.GetStamina > staminaLimit)
                {
                    toReturn = false;
                }
            }
        }

        return toReturn;
    }
}
