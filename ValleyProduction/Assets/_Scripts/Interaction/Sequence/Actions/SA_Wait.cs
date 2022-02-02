using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SA_Wait : InteractionActions
{
    private class WaiterData
    {
        public CPN_InteractionHandler caller;
        public TimerManager.Timer timer;
    }

    [SerializeField] private Vector2 waitTime;
    private List<WaiterData> waiters = new List<WaiterData>();

    protected override void OnEndAction(CPN_InteractionHandler caller)
    {
        for(int i = 0; i < waiters.Count; i++)
        {
            if(waiters[i].caller == caller)
            {
                waiters.RemoveAt(i);
                break;
            }
        }
    }

    protected override void OnInteruptAction(CPN_InteractionHandler caller)
    {
        for (int i = 0; i < waiters.Count; i++)
        {
            if (waiters[i].caller == caller)
            {
                waiters[i].timer.Stop();
                waiters.RemoveAt(i);
                break;
            }
        }
    }

    protected override void OnPlayAction(CPN_InteractionHandler caller)
    {
        WaiterData newWaiter = new WaiterData();
        newWaiter.caller = caller;
        newWaiter.timer = TimerManager.CreateTimer(Random.Range(waitTime.x, waitTime.y), () => EndAction(newWaiter.caller));

        waiters.Add(newWaiter);
    }
}
