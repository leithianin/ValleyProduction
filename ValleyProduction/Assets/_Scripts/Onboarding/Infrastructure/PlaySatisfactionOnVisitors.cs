using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySatisfactionOnVisitors : MonoBehaviour
{
    public void PlaySatisfaction()
    {
        List<VisitorBehavior> visitorsList = VisitorManager.GetUsedVisitors();

        foreach(VisitorBehavior vb in visitorsList)
        {
            vb.GetComponent<VLY_ComponentHandler>().GetComponentOfType<CPN_SatisfactionHandler>().AddSatisfaction(1f);
        }
    }
}
