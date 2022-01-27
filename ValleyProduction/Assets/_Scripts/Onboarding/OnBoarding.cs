using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class OnBoarding : MonoBehaviour
{
    protected Action callback;
    //OB_Sequence
    [SerializeField, Tooltip("Actions to play start unboarding")] private UnityEvent PlayOnStart;
    [SerializeField, Tooltip("Actions to play end unboarding")] private UnityEvent PlayOnEnd;

    protected abstract void OnPlay(); //Reset la sequence

    protected abstract void OnEnd();

    public void Play(Action callback_Play = null)
    {
        callback = callback_Play;
        PlayOnStart?.Invoke();
        OnPlay();
    }

    public void Over()
    {
        PlayOnEnd?.Invoke();
        OnEnd();
        callback?.Invoke();
    }
}
