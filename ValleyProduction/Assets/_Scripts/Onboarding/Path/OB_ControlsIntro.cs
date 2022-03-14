using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OB_ControlsIntro : OnBoarding
{
    public float time = 0.5f;

    private int actionNeeded = 3;
    private int actionDone = 0;

    public UnityEvent OnMouseScroll;
    public UnityEvent OnMoving;
    public UnityEvent OnRotateEvent;

    protected override void OnPlay()
    {
        PlayerInputManager.OnMouseScroll += OnScroll;
        PlayerInputManager.OnKeyMove += OnMove;
        SphericalTransform.OnMouseWheel += OnRotate;
        //PlayerInputManager.OnMouseScroll.AddListener(OnScroll);
    }

    protected override void OnEnd()
    {      
        
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
    
    public void OnRotate(float value)
    {
        if (value != 0)
        {
            SphericalTransform.OnMouseWheel -= OnRotate;
            OnRotateEvent?.Invoke();
            CheckAction();
        }
    }

    public void CheckAction()
    {
        actionDone++;
        if(actionDone == actionNeeded)
        {
            StartCoroutine(DesactivateAfterTime());
        }
    }

    IEnumerator DesactivateAfterTime()
    {
        yield return new WaitForSeconds(time);
        Over();
    }
}
