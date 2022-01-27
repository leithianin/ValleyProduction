using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IFB_DesactivateAfterTime : MonoBehaviour, IFeedbackPlayer
{
    public float timer = 0;

    public void Play()
    {
        gameObject.SetActive(false);
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
