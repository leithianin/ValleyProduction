using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OB_ControlsIntro : OnBoarding
{
    public UnityEvent OnMouseScroll;
    public UnityEvent OnMoving;

    public bool hasDoneAction = false;

    protected override void OnPlay()
    {
        PlayerInputManager.OnMouseScroll += OnScroll;
        PlayerInputManager.OnKeyMove += OnMove;
        //PlayerInputManager.OnMouseScroll.AddListener(OnScroll);
    }

    protected override void OnEnd()
    {      
        hasDoneAction = false;
    }

    public void OnScroll(float scrollValue)
    {
        OnMouseScroll?.Invoke();
        PlayerInputManager.OnMouseScroll -= OnScroll;
        CheckAction();
    }

    public void OnMove(Vector2 direction )
    {
        if (direction != Vector2.zero)
        {
            OnMoving?.Invoke();
            PlayerInputManager.OnKeyMove -= OnMove;
            CheckAction();
        }
    }

    public void CheckAction()
    {
        if(hasDoneAction)
        {
            Over();
        }
        else
        {
            hasDoneAction = true;
        }
    }
}
