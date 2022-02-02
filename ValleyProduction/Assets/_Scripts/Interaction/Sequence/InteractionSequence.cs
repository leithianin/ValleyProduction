using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSequence : InteractionActions
{
    private class SequenceHandler
    {
        public int currentSequenceIndex;
        public InteractionHandler caller;
    }

    [SerializeField] private List<InteractionActions> sequence;

    private List<SequenceHandler> sequenceUser = new List<SequenceHandler>();

    protected override void OnPlayAction(InteractionHandler caller)
    {
        SequenceHandler newHandler = new SequenceHandler();
        newHandler.currentSequenceIndex = -1;
        newHandler.caller = caller;

        sequenceUser.Add(newHandler);

        PlayNextStep(caller);
    }

    protected override void OnEndAction(InteractionHandler caller)
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

    private void PlayNextStep(InteractionHandler caller)
    {
        for (int i = 0; i < sequenceUser.Count; i++)
        {
            if (sequenceUser[i].caller == caller)
            {
                sequenceUser[i].currentSequenceIndex++;
                if (sequenceUser[i].currentSequenceIndex >= sequence.Count)
                {
                    EndAction(caller);
                }
                else
                {
                    sequence[sequenceUser[i].currentSequenceIndex].PlayAction(caller, () => PlayNextStep(caller));
                }
                break;
            }
        }
    }

    protected override void OnInteruptAction(InteractionHandler caller)
    {
        for (int i = 0; i < sequenceUser.Count; i++)
        {
            if (sequenceUser[i].caller == caller)
            {
                if (sequenceUser[i].currentSequenceIndex >= 0)
                {
                    sequence[sequenceUser[i].currentSequenceIndex].InteruptAction(caller);
                }
            }
            break;
        }
    }
}
