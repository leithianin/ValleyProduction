using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SA_PlaySound : InteractionActions
{
    [SerializeField] private AudioSound soundToPlay;
    [SerializeField] private AudioPlayer audioPlayer;

    protected override void OnEndAction(CPN_InteractionHandler caller)
    {
        
    }

    protected override void OnInteruptAction(CPN_InteractionHandler caller)
    {
        
    }

    protected override void OnPlayAction(CPN_InteractionHandler caller)
    {
        audioPlayer.Play(soundToPlay);

        EndAction(caller);
    }
}
