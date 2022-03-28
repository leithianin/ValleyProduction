using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CPN_ClicHandler : VLY_Component
{
    [SerializeField] private UnityEvent OnLeftMouseDown;
    [SerializeField] private UnityEvent OnRightMouseDown;
    [SerializeField] private UnityEvent PlayOnMouseEnter;
    [SerializeField] private UnityEvent PlayOnMouseExit;

    private void OnMouseDown()
    {
        if(Input.GetMouseButton(0))
        {
            OnLeftMouseDown?.Invoke();
        }
        else
        {
            OnRightMouseDown?.Invoke();
        }
    }

    private void OnMouseEnter()
    {
        PlayOnMouseEnter?.Invoke();
    }


    private void OnMouseExit()
    {
        PlayOnMouseExit?.Invoke();
    }
}
