using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VLY_FlagManager : VLY_Singleton<VLY_FlagManager>
{
    [SerializeField] private VLY_FlagList flagList;

    private static Dictionary<string, int> flags = new Dictionary<string, int>();

    public static Action<string, int> OnUpdateFlag;

    public static Action<string> OnTriggerFlag;

    private void Start()
    {
        //Ajoute tous les flags existant

        foreach(string flagName in flagList.Flags)
        {
            flags.Add(flagName, 0);
        }
    }

    public static void IncrementFlagValue(string flagName, int incrementValue)
    {
        if(flags.ContainsKey(flagName))
        {
            flags[flagName] += incrementValue;

            OnUpdateFlag?.Invoke(flagName, flags[flagName]);
        }
    }

    public static int GetFlagValue(string flagName)
    {
        if (flags.ContainsKey(flagName))
        {
            return flags[flagName];
        }
        return -1;
    }

    public static void TriggerFlag(string triggerName)
    {
        OnTriggerFlag?.Invoke(triggerName);
    }
}
