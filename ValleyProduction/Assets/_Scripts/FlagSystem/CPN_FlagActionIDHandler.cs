using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPN_FlagActionIDHandler : MonoBehaviour
{
    [SerializeField] private string flagToTrigger;

    public void TriggerFlag(string flagName)
    {
        VLY_FlagManager.TriggerFlag(flagName);
    }

    public void TriggerFlag()
    {
        TriggerFlag(flagToTrigger);
    }
}
