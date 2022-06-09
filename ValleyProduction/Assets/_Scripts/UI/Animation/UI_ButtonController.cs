using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_ButtonController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private bool isSelected = false;

    public void SetBool(string boolName)
    {
        animator.SetBool(boolName, true);
    }

    public void UnsetBool(string boolName)
    {
        animator.SetBool(boolName, false);
    }

    public void SetSelected()
    {
        SetSelected(!isSelected);
    }

    public void SetSelected(bool value)
    {
        isSelected = value;

        if (isSelected)
        {
            animator.SetBool("Selected", true);
        }
        else
        {
            animator.SetBool("Selected", false);
        }
    }
}
