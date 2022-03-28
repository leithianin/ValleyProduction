using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CPN_ClicHandler : VLY_Component
{
    [SerializeField] private UnityEvent OnLeftMouseDown;
    [SerializeField] private UnityEvent OnRightMouseDown;

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
        
    }


    private void OnMouseExit()
    {

    }
}
