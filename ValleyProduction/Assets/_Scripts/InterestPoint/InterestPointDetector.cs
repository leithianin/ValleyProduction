using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterestPointDetector : MonoBehaviour
{
    public Action<InterestPoint> OnDiscoverInterestPoint;
    public Action<InterestPoint> OnRemoveInterestPoint;

    private void OnTriggerEnter(Collider other)
    {
        InterestPoint interestPoint = other.GetComponent<InterestPoint>();
        if(interestPoint != null)
        {
            OnDiscoverInterestPoint?.Invoke(interestPoint);
            interestPoint.OnDisableInterestPoint += RemoveInterestPoint;
        }
    }

    private void RemoveInterestPoint(InterestPoint toRemove)
    {
        OnRemoveInterestPoint?.Invoke(toRemove);
        toRemove.OnDisableInterestPoint -= RemoveInterestPoint;
    }
}
