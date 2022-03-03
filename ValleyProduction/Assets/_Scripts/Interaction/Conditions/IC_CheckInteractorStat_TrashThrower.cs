using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IC_CheckInteractorStat_TrashThrower : InteractionCondition
{
    [SerializeField, Tooltip("If less than 0, will not be check.")] private float timeBeforeThrow;

    public override bool IsConditionTrue(CPN_InteractionHandler interacter)
    {
        bool toReturn = true;

        if (interacter.HasComponent<CPN_TrashThrower>(out CPN_TrashThrower thrower))
        {
            if(timeBeforeThrow >= 0)
            {
                if(thrower.TimeBeforeThrow() >= timeBeforeThrow)
                {
                    toReturn = false;
                }
            }
        }

        return toReturn;
    }
}
