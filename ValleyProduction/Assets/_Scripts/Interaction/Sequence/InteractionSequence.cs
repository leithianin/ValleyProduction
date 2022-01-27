using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSequence : InteractionActions
{
    [SerializeField] private List<InteractionActions> sequence;

    private int currentSequenceIndex = -1;

    protected override void OnPlayAction(InteractionHandler caller)
    {
        currentSequenceIndex = -1;

        PlayNextStep(caller);
    }

    protected override void OnEndAction(InteractionHandler caller)
    {
        
    }

    private void PlayNextStep(InteractionHandler caller)
    {
        currentSequenceIndex++;
        if (currentSequenceIndex >= sequence.Count)
        {
            EndAction(caller);
        }
        else
        {
            sequence[currentSequenceIndex].PlayAction(caller, () => PlayNextStep(caller));
        }
    }

    protected override void OnInteruptAction(InteractionHandler caller)
    {
        if (currentSequenceIndex >= 0)
        {
            sequence[currentSequenceIndex].InteruptAction(caller);
        }
    }
}
