using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalBehavior : MonoBehaviour
{
    [SerializeField] private InteractionHandler interaction;
    [SerializeField] private InteractionSequence sequence;

    private void Start()
    {
        PlaySequence();
    }

    private void PlaySequence()
    {
        PlaySequenceDelay();
    }

    private void PlaySequenceDelay()
    {
        TimerManager.CreateTimer(Time.fixedDeltaTime, () => sequence.PlayAction(interaction, PlaySequence));
    }
}
