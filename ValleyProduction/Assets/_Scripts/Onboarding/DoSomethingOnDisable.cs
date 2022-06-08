using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoSomethingOnDisable : MonoBehaviour
{
    public UnityEvent OnDisableEvent;

    public void OnDisable()
    {
        OnDisableEvent?.Invoke();
    }
}
