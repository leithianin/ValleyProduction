using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractionActions : MonoBehaviour
{
    public class SequenceCallback
    {
        public Action endCallback;
        public Action interuptCallback;
        public CPN_InteractionHandler caller;

        public SequenceCallback(Action nEndCallbeck, Action nInteruptCallback, CPN_InteractionHandler nCaller)
        {
            endCallback = nEndCallbeck;
            interuptCallback = nInteruptCallback;
            caller = nCaller;
        }
    }

    private bool isSecondaryAction = false;

    protected List<SequenceCallback> askedCallbacks = new List<SequenceCallback>();

    protected abstract void OnPlayAction(CPN_InteractionHandler caller);

    protected abstract void OnEndAction(CPN_InteractionHandler caller);

    protected abstract void OnInteruptAction(CPN_InteractionHandler caller);

    public void PlayAction(CPN_InteractionHandler caller, Action endCallback, Action interuptCallback, bool secondaryAction)
    {
        if (caller != null)
        {
            isSecondaryAction = secondaryAction;

            if (!secondaryAction)
            {
                askedCallbacks.Add(new SequenceCallback(endCallback, interuptCallback, caller));
            }

            OnPlayAction(caller);
        }
    }

    public void EndAction(CPN_InteractionHandler caller)
    {
        if (caller != null)
        {
            OnEndAction(caller);

            if(!isSecondaryAction)
            { 
                List<SequenceCallback> callbacksToTry = new List<SequenceCallback>(askedCallbacks);

                for (int i = 0; i < callbacksToTry.Count; i++)
                {
                    if (callbacksToTry[i].caller == caller)
                    {
                        callbacksToTry[i].endCallback?.Invoke();
                        callbacksToTry[i].endCallback = null;

                        if (askedCallbacks.Count > i)
                        {
                            askedCallbacks.RemoveAt(i);
                        }

                        callbacksToTry.RemoveAt(i);
                        i--;
                    }
                }
            }
        }
    }

    public void InteruptAction(CPN_InteractionHandler caller)
    {
        OnInteruptAction(caller);

        if (!isSecondaryAction)
        {
            for (int i = 0; i < askedCallbacks.Count; i++)
            {
                if (askedCallbacks[i].caller == caller)
                {
                    askedCallbacks[i].interuptCallback?.Invoke();
                    askedCallbacks[i].interuptCallback = null;

                    askedCallbacks[i].endCallback = null;

                    askedCallbacks.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
