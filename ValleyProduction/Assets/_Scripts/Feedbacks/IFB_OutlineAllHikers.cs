using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IFB_OutlineAllHikers : MonoBehaviour,IFeedbackPlayer
{

    public void Play()
    {
        List<VisitorBehavior> hikersList = VisitorManager.HikersList();

        foreach(VisitorBehavior vb in hikersList)
        {
            vb.Handler.BlinkOnOutline();
        }
    }

    public void Stop()
    {
        List<VisitorBehavior> hikersList = VisitorManager.HikersList();

        foreach (VisitorBehavior vb in hikersList)
        {
            vb.Handler.BlinkOffOutline();
        }
    }
}
