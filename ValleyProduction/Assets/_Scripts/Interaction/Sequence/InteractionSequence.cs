using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSequence : InteractionActions
{
    [Serializable]
    private class SequenceHandler
    {
        public int currentSequenceIndex;
        public CPN_InteractionHandler caller;
    }

    [Serializable]
    private class CompleteSequenceAction
    {
        public InteractionActions mainAction;
        public List<InteractionActions> secondaryActions = new List<InteractionActions>();
    }

    [SerializeField] private List<CompleteSequenceAction> sequence;

    [SerializeField] private List<SequenceHandler> sequenceUser = new List<SequenceHandler>();

    private List<CPN_InteractionHandler> callersForNextFrame = new List<CPN_InteractionHandler>();

    protected override void OnPlayAction(CPN_InteractionHandler caller)
    {
        SequenceHandler newHandler = new SequenceHandler();

        for (int i = 0; i < sequenceUser.Count; i++)
        {
            if(sequenceUser[i].caller == caller)
            {
                newHandler = null;
                sequenceUser[i].currentSequenceIndex = -1;
                sequenceUser[i].caller = caller;
            }
        }

        if (newHandler != null)
        {
            newHandler.currentSequenceIndex = -1;
            newHandler.caller = caller;

            sequenceUser.Add(newHandler);
        }

        PlayNextStep(caller);
    }

    protected override void OnEndAction(CPN_InteractionHandler caller)
    {
        for (int i = 0; i < sequenceUser.Count; i++)
        {
            if (sequenceUser[i].caller == caller)
            {
                sequenceUser.RemoveAt(i);
                break;
            }
        }
    }

    private void SetNextStep(CPN_InteractionHandler caller)
    {
        if (!callersForNextFrame.Contains(caller))
        {
            callersForNextFrame.Add(caller);
        }
    }

    private void LateUpdate()
    {
        foreach(CPN_InteractionHandler caller in callersForNextFrame)
        {
            PlayNextStep(caller);
        }

        callersForNextFrame.Clear();
    }

    private void PlayNextStep(CPN_InteractionHandler caller)
    {
        for (int i = 0; i < sequenceUser.Count; i++)
        {
            if (sequenceUser[i].caller == caller)
            {
                SequenceHandler handler = sequenceUser[i];

                handler.currentSequenceIndex++;
                if (handler.currentSequenceIndex >= sequence.Count)
                {
                    EndAction(caller);
                }
                else
                {
                    for (int j = 0; j < sequence[handler.currentSequenceIndex].secondaryActions.Count; j++)
                    {
                        sequence[handler.currentSequenceIndex].secondaryActions[j].PlayAction(caller, () => SetNextStep(caller), null, true);
                    }

                    sequence[handler.currentSequenceIndex].mainAction.PlayAction(caller, () => SetNextStep(caller), null, false);
                }
                break;
            }
        }
    }

    protected override void OnInteruptAction(CPN_InteractionHandler caller)
    {
        for (int l = 0; l < sequenceUser.Count; l++)
        {
            if (sequenceUser[l].caller == caller)
            {
                if (sequenceUser[l].currentSequenceIndex >= 0)
                {
                    sequence[sequenceUser[l].currentSequenceIndex].mainAction.InteruptAction(caller);

                    for (int j = 0; j < sequence[sequenceUser[l].currentSequenceIndex].secondaryActions.Count; j++)
                    {
                        sequence[sequenceUser[l].currentSequenceIndex].secondaryActions[j].InteruptAction(caller);
                    }

                    sequenceUser.RemoveAt(l);
                    l--;
                }
            }
            break;
        }
    }
}
