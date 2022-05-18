using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VLY_FlagManager : VLY_Singleton<VLY_FlagManager>
{
    [SerializeField] private VLY_FlagList flagList;

    private static Dictionary<string, int> flags = new Dictionary<string, int>();

    private static Dictionary<string, List<Action<int>>> incrementalFlagListeners = new Dictionary<string, List<Action<int>>>();
    private static Dictionary<string, List<Action>> triggerFlagListeners = new Dictionary<string, List<Action>>();

    /*public static Action<string, int> OnUpdateFlag;

    public static Action<string> OnTriggerFlag;*/

    protected override void OnAwake()
    {
        //Ajoute tous les flags existant

        foreach(string flagName in flagList.IncrementalsFlags)
        {
            flags.Add(flagName, 0);

            incrementalFlagListeners.Add(flagName, new List<Action<int>>());
        }

        foreach(string flagName in flagList.TriggerFlags)
        {
            triggerFlagListeners.Add(flagName, new List<Action>());
        }
    }

    public static void AddIncrementFlagListener(string flagName, Action<int> callback)
    {
        if(!incrementalFlagListeners[flagName].Contains(callback))
        {
            incrementalFlagListeners[flagName].Add(callback);
        }
    }
    public static void RemoveIncrementFlagListener(string flagName, Action<int> callback)
    {
        if (incrementalFlagListeners[flagName].Contains(callback))
        {
            incrementalFlagListeners[flagName].Remove(callback);
        }
    }

    public static void AddTriggerFlagListener(string flagName, Action callback)
    {
        if (!triggerFlagListeners[flagName].Contains(callback))
        {
            triggerFlagListeners[flagName].Add(callback);
        }
    }
    public static void RemoveTriggerFlagListener(string flagName, Action callback)
    {
        if (triggerFlagListeners[flagName].Contains(callback))
        {
            triggerFlagListeners[flagName].Remove(callback);
        }
    }


    public static void IncrementFlagValue(string flagName, int incrementValue)
    {
        if(flags.ContainsKey(flagName))
        {
            flags[flagName] += incrementValue;

            foreach(Action<int> act in incrementalFlagListeners[flagName])
            {
                act?.Invoke(flags[flagName]);
            }
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
        foreach(Action act in triggerFlagListeners[triggerName])
        {
            act?.Invoke();
        }
    }
}
