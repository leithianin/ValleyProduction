using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IFB_DesactivateAfterTime : MonoBehaviour, IFeedbackPlayer
{
    private float timer = 0;

    [SerializeField] private bool isRealTimer = false;
    public Animator animator;

    public void Play()
    {
        gameObject.SetActive(false);
    }

    public void Play(float integer)
    {
        timer = integer;
        if(animator != null)
        {
            animator.SetTrigger("Over");
        }

        //StartCoroutine(PlayAfter()); CODE REVIEW
        if(isRealTimer)
        {
            TimerManager.CreateRealTimer(timer, Play);
        }
        else
        {
            TimerManager.CreateGameTimer(timer, Play);
        }
    }

    IEnumerator PlayAfter()
    {
        Debug.Log("OK");
        yield return new WaitForSeconds(timer);
        Play();
    }
}
