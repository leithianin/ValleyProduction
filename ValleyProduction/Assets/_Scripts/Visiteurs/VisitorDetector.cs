using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VisitorDetector : MonoBehaviour
{
    List<VisitorBehavior> visitorInArea = new List<VisitorBehavior>();

    [SerializeField] private UnityEvent<VLY_ComponentHandler> OnVisitorEnter;
    [SerializeField] private UnityEvent<VLY_ComponentHandler> OnVisitorExit;

    private void OnTriggerEnter(Collider other)
    {
        VisitorBehavior visitor = other.GetComponent<VisitorBehavior>();

        if (visitor != null && !visitorInArea.Contains(visitor))
        {
            visitorInArea.Add(visitor);
            OnVisitorEnter?.Invoke(visitor.Handler);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        VisitorBehavior visitor = other.GetComponent<VisitorBehavior>();
        if (visitor != null && visitorInArea.Contains(visitor))
        {
            visitorInArea.Remove(visitor);
            OnVisitorExit?.Invoke(visitor.Handler);
        }
    }
}
