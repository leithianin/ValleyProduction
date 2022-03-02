using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SA_PlayAnimation : InteractionActions
{
    private struct PlayedAnimation
    {
        public CPN_InteractionHandler caller;
        public TimerManager.Timer timer;

        public PlayedAnimation(CPN_InteractionHandler c, TimerManager.Timer t)
        {
            caller = c;
            timer = t;
        }
    }

    [SerializeField] private string animationName;
    [SerializeField] private float timePlayed;

    private List<PlayedAnimation> animationPlayed = new List<PlayedAnimation>();

    protected override void OnPlayAction(CPN_InteractionHandler caller)
    {
        AnimationHandler animHandler = null;

        if(caller.HasComponent<AnimationHandler>(ref animHandler))
        {
            // Jouer animation

            TimerManager.Timer newAnimTimer = TimerManager.CreateGameTimer(timePlayed, () => EndAction(caller));

            animationPlayed.Add(new PlayedAnimation(caller, newAnimTimer));
        }
        else
        {
            EndAction(caller);
        }
    }

    protected override void OnEndAction(CPN_InteractionHandler caller)
    {
        // Stoper l'animation

        for (int i = 0; i < animationPlayed.Count; i++)
        {
            if (animationPlayed[i].caller == caller)
            {
                animationPlayed.RemoveAt(i);
                break;
            }
        }

    }

    protected override void OnInteruptAction(CPN_InteractionHandler caller)
    {
        // Stoper l'animation

        for (int i = 0; i < animationPlayed.Count; i++)
        {
            if (animationPlayed[i].caller == caller)
            {
                animationPlayed[i].timer.Stop();
                animationPlayed.RemoveAt(i);
                break;
            }
        }
    }
}
