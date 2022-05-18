using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IFB_OnEnableDoSomething : MonoBehaviour, IFeedbackPlayer
{
    public UnityEvent OnEnableEvent;

    public void OnEnable()
    {
        Play();
    }

    public void Play()
    {
        OnEnableEvent?.Invoke();
    }
}
