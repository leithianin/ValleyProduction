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

    [Serializable]
    private class CompleteSequenceAction
    {
        public InteractionActions mainAction;
        public List<InteractionActions> secondaryActions;
    }

    [SerializeField] private List<CompleteSequenceAction> sequence;

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
                    sequence[sequenceUser[i].currentSequenceIndex].mainAction.PlayAction(caller, () => PlayNextStep(caller), null, false);

                    for(int j = 0; j < sequence[sequenceUser[i].currentSequenceIndex].secondaryActions.Count; j++)
                    {
                        sequence[sequenceUser[i].currentSequenceIndex].secondaryActions[j].PlayAction(caller, () => PlayNextStep(caller), null, true);
                    }
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
