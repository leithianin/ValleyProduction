using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IFB_ActivateAfterTime : MonoBehaviour, IFeedbackPlayer
{
    private float timer = 0;
    public List<GameObject> goList = new List<GameObject>();

    public void Play()
    {
        foreach (GameObject go in goList)
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