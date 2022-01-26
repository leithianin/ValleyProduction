using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SA_Wait : InteractionActions
{
    private class WaiterData
    {
        public InteractionHandler caller;
        public TimerManager.Timer timer;
    }

    [SerializeField] private Vector2 waitTime;
    private List<WaiterData> waiters = new List<WaiterData>();

    protected override void OnEndAction(InteractionHandler caller)
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

    protected override void OnInteruptAction(InteractionHandler caller)
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

    protected override void OnPlayAction(InteractionHandler caller)
    {
        WaiterData newWaiter = new WaiterData();
        newWaiter.caller = caller;
        newWaiter.timer = TimerManager.CreateTimer(Random.Range(waitTime.x, waitTime.y), () => EndAction(newWaiter.caller));

        waiters.Add(newWaiter);
    }
}
