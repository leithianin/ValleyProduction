using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IFB_OutlineAllTourist : MonoBehaviour,IFeedbackPlayer
{
    public bool stopped = false;
    public void Play()
    {
        if (!stopped)
        {
            List<VisitorBehavior> touristList = VisitorManager.TouristList();

            foreach (VisitorBehavior vb in touristList)
            {
                vb.Handler.BlinkOnOutline();
            }

            TimerManager.CreateRealTimer(1f, Play);
        }
    }

    public void Stop()
    {
        stopped = true;

        List<VisitorBehavior> touristList = VisitorManager.TouristList();

        foreach (VisitorBehavior vb in touristList)
        {
            vb.Handler.BlinkOffOutline();
        }
    }
}
