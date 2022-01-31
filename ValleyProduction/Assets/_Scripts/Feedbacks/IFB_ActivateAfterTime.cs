using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IFB_ActivateAfterTime : MonoBehaviour, IFeedbackPlayer
{
    private float timer = 0;
    public GameObject go = null;

    public void Play()
    {
        if (go != null)
        {
            go.SetActive(true);
        }
        else
        {
            gameObject.SetActive(true);
        }
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