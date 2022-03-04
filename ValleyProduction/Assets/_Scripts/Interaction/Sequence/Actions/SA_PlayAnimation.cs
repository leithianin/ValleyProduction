using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SA_PlayAnimation : InteractionActions
{
    private struct PlayedAnimation
    {
        public CPN_InteractionHandler caller;
        public TimerManager.Timer timer;
        public BodyAnimationType animationType;

        public PlayedAnimation(CPN_InteractionHandler c, TimerManager.Timer t, BodyAnimationType nAnimationType)
        {
            caller = c;
            timer = t;
            animationType = nAnimationType;
        }
    }

    [SerializeField] private BodyAnimationType animationToPlay;
    [SerializeField] private float timePlayed;
    [SerializeField] private bool stopOnEnd;

    private List<PlayedAnimation> animationPlayed = new List<PlayedAnimation>();

    protected override void OnPlayAction(CPN_InteractionHandler caller)
    {
        if(caller.HasComponent<AnimationHandler>(out AnimationHandler animHandler))
        {
            animHandler.PlayBodyAnim(animationToPlay);

            TimerManager.Timer newAnimTimer = TimerManager.CreateGameTimer(timePlayed, () => EndAction(caller));

            animationPlayed.Add(new PlayedAnimation(caller, newAnimTimer, animationToPlay));
        }
        else
        {
            EndAction(caller);
        }
    }

    protected override void OnEndAction(CPN_InteractionHandler caller)
    {
        for (int i = 0; i < animationPlayed.Count; i++)
        {
            if (animationPlayed[i].caller == caller)
            {
                if (stopOnEnd && caller.HasComponent<AnimationHandler>(out AnimationHandler animHandler))
                {
                    animHandler.StopBodyAnim(animationToPlay);
                }

                animationPlayed.RemoveAt(i);
                break;
            }
        }

    }

    protected override void OnInteruptAction(CPN_InteractionHandler caller)
    {
        for (int i = 0; i < animationPlayed.Count; i++)
        {
            if (animationPlayed[i].caller == caller)
            {
                if (caller.HasComponent<AnimationHandler>(out AnimationHandler animHandler))
                {
                    animHandler.StopBodyAnim(animationToPlay);
                }

                animationPlayed[i].timer.Stop();
                animationPlayed.RemoveAt(i);
                break;
            }
        }
    }
}
