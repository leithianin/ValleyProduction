using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractionActions : MonoBehaviour
{
    public class SequenceCallback
    {
        public Action callback;
        public InteractionHandler caller;

        public SequenceCallback(Action nCallback, InteractionHandler nCaller)
        {
            callback = nCallback;
            caller = nCaller;
        }
    }

    protected List<SequenceCallback> askedCallbacks = new List<SequenceCallback>();

    protected abstract void OnPlayAction(InteractionHandler caller);

    protected abstract void OnEndAction(InteractionHandler caller);

    public void PlayAction(InteractionHandler caller, Action callback)
    {
        askedCallbacks.Add(new SequenceCallback(callback, caller));

        Debug.Log(caller);
        OnPlayAction(caller);
    }

    public void EndAction(InteractionHandler caller)
    {
        OnEndAction(caller);
        for (int i = 0; i < askedCallbacks.Count; i++)
        {
            if (askedCallbacks[i].caller == caller)
            {
                askedCallbacks[i].callback?.Invoke();

                askedCallbacks.RemoveAt(i);
                i--;
            }
        }

    }
}
