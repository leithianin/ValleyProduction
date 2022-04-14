using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPN_FlagHandler : MonoBehaviour
{
    [SerializeField] private string flagName;

    [ContextMenu("Increment Flag")]
    public void IncrementFlag()
    {
        VLY_FlagManager.IncrementFlagValue(flagName);
    }

    public void IncrementFlag(string nFlagName)
    {
        VLY_FlagManager.IncrementFlagValue(nFlagName);
    }
}
