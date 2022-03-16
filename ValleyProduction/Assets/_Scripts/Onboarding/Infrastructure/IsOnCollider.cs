using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsOnCollider : MonoBehaviour
{
    public Collider collider;

    public void OnTriggerEnter(Collider other)
    {
        if (other == collider)
        {
            OnBoardingManager.OnClickZone?.Invoke(true);
        }
    }
}

