using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOverAfterTime : MonoBehaviour, IFeedbackPlayer
{
    private float timer = 0;
    public OnBoarding onboarding;

    public void Play()
    {
        onboarding.Over();
    }

    public void Play(float integer)
    {
        timer = integer;
        StartCoroutine(PlayAfter());
    }

    IEnumerator PlayAfter()
    {
        yield return new WaitForSeconds(timer);
        Play();
    }
}
