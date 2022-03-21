using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSequence : InteractionActions
{
    private class SequenceHandler
    {
        public int currentSequenceIndex;
        public CPN_InteractionHandler caller;
    }

    [SerializeField] private List<InteractionActions> sequence;

    private List<SequenceHandler> sequenceUser = new List<SequenceHandler>();

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

    private void PlayNextStep(CPN_InteractionHandler caller)
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
                    sequence[sequenceUser[i].currentSequenceIndex].PlayAction(caller, () => PlayNextStep(caller), null);
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
                    sequence[sequenceUser[l].currentSequenceIndex].InteruptAction(caller);
                    sequenceUser.RemoveAt(l);
                    l--;
                }
            }
            break;
        }
    }
}
