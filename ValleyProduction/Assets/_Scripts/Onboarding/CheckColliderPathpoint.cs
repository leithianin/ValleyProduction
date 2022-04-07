using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckColliderPathpoint : MonoBehaviour
{
    public UnityEvent OnCollide;

    private void OnEnable()
    {
        
    }

    private void Start()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<IST_PathPoint>() && enabled)
        {
            OnCollide?.Invoke();
            enabled = false;
        }
    }
}
