using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorChangeValue_Pollution : MonoBehaviour
{
    [SerializeField] private float pollutionThrowDelay;

    public void DelayPollutionThrowFromHandler(VLY_ComponentHandler handler)
    {
        CPN_TrashThrower thrower = handler.GetComponentOfType<CPN_TrashThrower>();

        if (thrower != null)
        {
            thrower.DelayThrow(pollutionThrowDelay);
        }
    }
}
