using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterestPointCollider : MonoBehaviour
{
    public PathFragmentData pfdRef;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<InterestPoint>())
        {
            pfdRef.AddInterestPoint(gameObject.GetComponent<InterestPoint>());
        }
    }
}
