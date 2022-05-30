using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetExplicationDone : MonoBehaviour
{
    public Animator animator;
    public TextMeshProUGUI objectiveText;

    public void SetToDone()
    {
        if(gameObject.activeSelf)
        {
            objectiveText.text = "<s>" + objectiveText.text + "</s>";
            animator.SetTrigger("Exit");
        }
    }
}
