using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckColliderPathpoint : MonoBehaviour
{
    [SerializeField] private UnityEvent OnCollide;

   /* private void OnTriggerEnter(Collider other)
    {
        IST_PathPoint pathpoint = other.gameObject.GetComponent<IST_PathPoint>();

        if (pathpoint != null && InfrastructureManager.GetMovedObject == null)
        {
            Debug.Log("OnCollide");
            OnCollide?.Invoke();
            enabled = false;
        }
    }*/

    private void OnTriggerStay(Collider other)
    {
        IST_PathPoint pathpoint = other.gameObject.GetComponent<IST_PathPoint>();

        if (pathpoint != null && InfrastructureManager.GetMovedObject == null)
        {
            Debug.Log("OnCollide");
            OnCollide?.Invoke();
            enabled = false;
        }
    }
}
