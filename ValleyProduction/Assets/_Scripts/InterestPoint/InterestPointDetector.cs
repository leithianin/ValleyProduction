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
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InterestPoint interestPoint = other.GetComponent<InterestPoint>();
        if (interestPoint != null)
        {
            OnRemoveInterestPoint?.Invoke(interestPoint);
        }
    }
}
