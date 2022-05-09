using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UI_Animator : MonoBehaviour
{
    public Animator animator;
    bool isSelected;

    public UnityEvent OnDisableEvent;

    private void Start()
    {
        isSelected = animator.GetBool("Selected");
    }

    private void OnDisable()
    {
        OnDisableEvent?.Invoke();
    }

    public void SetHighlighted(bool cond)
    {
        animator.SetBool("Highlighted", cond);
    }

    public void SetSelected(bool cond)
    {
        animator.SetBool("Selected", cond);
    }

    public void SetPressed(bool cond)
    {
        animator.SetBool("Pressed", cond);
    }
}
