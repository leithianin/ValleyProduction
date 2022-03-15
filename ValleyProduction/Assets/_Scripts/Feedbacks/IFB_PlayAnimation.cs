using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IFB_PlayAnimation : MonoBehaviour, IFeedbackPlayer
{
    [SerializeField] private string animationName;
    [SerializeField] private Animator animator;

    public void Play()
    {
        animator.Play(animationName);
    }

    public void Play(string animationToPlay)
    {
        animationName = animationToPlay;
        Play();
    }
}
