using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CPN_TriggerFlagListener : MonoBehaviour
{
    [SerializeField] private string flagToTrigger;
    [SerializeField] private UnityEvent OnTriggerFlagEvent;

    private void OnEnable()
    {
        VLY_FlagManager.OnTriggerFlag += OnTriggerFlag;
    }

    private void OnDisable()
    {
        VLY_FlagManager.OnTriggerFlag -= OnTriggerFlag;
    }

    public void OnTriggerFlag(string flag)
    {
        if (flag == flagToTrigger)
        {
            OnTriggerFlagEvent?.Invoke();
        }
    }
}
