using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalAnimationHandler : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void SetWalking(bool isWalking)
    {
        animator.SetBool("IsWalking", isWalking);
    }
}
