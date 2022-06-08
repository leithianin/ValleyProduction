using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IFB_DoSomethingAfterTime : MonoBehaviour, IFeedbackPlayer
{
    public UnityEvent DoThis;

    public void Play()
    {
        DoThis?.Invoke();
    }

    public void Play(int time)
    {
        TimerManager.CreateRealTimer(time, Play);
    }
}
