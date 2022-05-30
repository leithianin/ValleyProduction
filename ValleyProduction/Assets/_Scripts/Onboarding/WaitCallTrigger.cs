using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaitCallTrigger : MonoBehaviour
{
    public UnityEvent Play;
    
    public void WaitTime(float i)
    {
        TimerManager.CreateRealTimer(i, () => Play?.Invoke());
    }
}
