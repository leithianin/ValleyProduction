using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IFB_SetAnimationParameter : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void SetTrigger(string triggerName)
    {
        animator.SetTrigger(triggerName);
    }

    public void UnsetTrigger(string triggerName)
    {
        animator.ResetTrigger(triggerName);
    }

    public void SetBool(string boolName)
    {
        animator.SetBool(boolName, true);
    }

    public void UnsetBool(string boolName)
    {
        animator.SetBool(boolName, false);
    }
}
