using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayAnimation : MonoBehaviour
{
    public UnityEvent OnEnableEvent;

    private void OnEnable()
    {
        OnEnableEvent?.Invoke();
    }

    private void OnDisable()
    {
        enabled = false;
    }
}
