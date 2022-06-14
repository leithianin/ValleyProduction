using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IFB_DoSomething : MonoBehaviour, IFeedbackPlayer
{
    public UnityEvent DoThis;

    public void Play()
    {
        if (gameObject.activeSelf)
        {
            DoThis?.Invoke();
        }
    }

    public void PlayGlobal()
    {
        gameObject.transform.parent = null;

        Play();
    }
}
