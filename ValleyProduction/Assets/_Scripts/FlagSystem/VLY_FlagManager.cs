using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VLY_FlagManager : VLY_Singleton<VLY_FlagManager>
{
    private static Dictionary<string, int> flags = new Dictionary<string, int>();

    public static Action<string, int> OnUpdateFlag;

    private void Start()
    {
        //Ajoute tous les flags existant

        for(int i = 0; i < 1000; i++)
        {
            flags.Add(i.ToString(), 0);
        }

        flags.Add("RebuildBridge", 0);
    }

    public static void IncrementFlagValue(string flagName)
    {
        if(flags.ContainsKey(flagName))
        {
            flags[flagName]++;

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
}
