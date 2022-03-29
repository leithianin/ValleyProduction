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

    public void MouseDown(int mouseID)
    {
        switch(mouseID)
        {
            case 0:
                OnLeftMouseDown?.Invoke();
                break;
            case 1:
                OnRightMouseDown?.Invoke();
                break;
        }
    }

    public void MouseEnter()
    {
        PlayOnMouseEnter?.Invoke();
    }

    public void MouseExit()
    {
        PlayOnMouseExit?.Invoke();
    }
}
