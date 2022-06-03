using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IFB_OutlineAllTourist : MonoBehaviour,IFeedbackPlayer
{

    public void Play()
    {
        List<VisitorBehavior> touristList = VisitorManager.TouristList();

        foreach(VisitorBehavior vb in touristList)
        {
            vb.Handler.BlinkOnOutline();
        }
    }

    public void Stop()
    {
        List<VisitorBehavior> touristList = VisitorManager.TouristList();

        foreach (VisitorBehavior vb in touristList)
        {
            vb.Handler.BlinkOffOutline();
        }
    }
}
