using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractionActions : MonoBehaviour
{
    public class SequenceCallback
    {
        public Action callback;
        public CPN_InteractionHandler caller;

        public SequenceCallback(Action nCallback, CPN_InteractionHandler nCaller)
        {
            callback = nCallback;
            caller = nCaller;
        }
    }

    protected List<SequenceCallback> askedCallbacks = new List<SequenceCallback>();

    protected abstract void OnPlayAction(CPN_InteractionHandler caller);

    protected abstract void OnEndAction(CPN_InteractionHandler caller);

    protected abstract void OnInteruptAction(CPN_InteractionHandler caller);

    public void PlayAction(CPN_InteractionHandler caller, Action callback)
    {
        askedCallbacks.Add(new SequenceCallback(callback, caller));

        OnPlayAction(caller);
    }

    public void EndAction(CPN_InteractionHandler caller)
    {
        OnEndAction(caller);

        List<SequenceCallback> callbacksToTry = new List<SequenceCallback>(askedCallbacks);

        for (int i = 0; i < callbacksToTry.Count; i++)
        {
            if (callbacksToTry[i].caller == caller)
            {
                callbacksToTry[i].callback?.Invoke();
                callbacksToTry[i].callback = null;

                if (askedCallbacks.Count > i)
                {
                    askedCallbacks.RemoveAt(i);
                }

                callbacksToTry.RemoveAt(i);
                i--;
            }
        }
    }

    public void InteruptAction(CPN_InteractionHandler caller)
    {
        OnInteruptAction(caller);

        for (int i = 0; i < askedCallbacks.Count; i++)
        {
            if (askedCallbacks[i].caller == caller)
            {
                askedCallbacks[i].callback = null;

                askedCallbacks.RemoveAt(i);
                i--;
            }
        }
    }
}
