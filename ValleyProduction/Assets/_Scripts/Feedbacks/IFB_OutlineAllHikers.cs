using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IFB_OutlineAllHikers : MonoBehaviour,IFeedbackPlayer
{
    private bool end = false;

    public void Play()
    {
        List<VisitorBehavior> hikersList = VisitorManager.HikersList();

        foreach(VisitorBehavior vb in hikersList)
        {
            vb.Handler.BlinkOnOutline();
        }

        if (!end)
        {
            TimerManager.CreateRealTimer(1f, Play);
        }
    }

    public void Stop()
    {
        end = true;
        List<VisitorBehavior> hikersList = VisitorManager.HikersList();

        foreach (VisitorBehavior vb in hikersList)
        {
            vb.Handler.BlinkOffOutline();
        }
    }
}
