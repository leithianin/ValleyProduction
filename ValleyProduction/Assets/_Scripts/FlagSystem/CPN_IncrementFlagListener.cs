using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CPN_IncrementFlagListener : MonoBehaviour
{
    [SerializeField] private string flagToCheck;
    [SerializeField] private int flagValueWanted;
    [SerializeField] private UnityEvent OnIncrementFlagEvent;

    private void OnEnable()
    {
        VLY_FlagManager.AddIncrementFlagListener(flagToCheck, OnTriggerFlag);
    }

    private void OnDisable()
    {
        VLY_FlagManager.RemoveIncrementFlagListener(flagToCheck, OnTriggerFlag);
    }

    public void OnTriggerFlag(int value)
    {
        if (value == flagValueWanted)
        {
            OnIncrementFlagEvent?.Invoke();
        }
    }
}
