using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnAllObjectiveDone : MonoBehaviour
{
    public List<SetObjectiveDone> objectiveList = new List<SetObjectiveDone>();

    public UnityEvent OnDone;
    public UnityEvent OnUndone;

    public void AreObjectivesDone()
    {
        foreach(SetObjectiveDone sod in objectiveList)
        {
            if(!sod.checkImage.enabled)
            {
                OnUndone?.Invoke();
                return;
            }
        }

        OnDone?.Invoke();
    }
}
