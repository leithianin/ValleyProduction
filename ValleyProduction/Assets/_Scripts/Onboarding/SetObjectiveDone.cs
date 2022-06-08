using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetObjectiveDone : MonoBehaviour
{
    public GameObject parent;
    public TextMeshProUGUI objectiveText;
    public Image checkImage;

    public void SetObjectiveToDone()
    {
        if (parent.gameObject.activeSelf)
        {
            objectiveText.text = "<s>" + objectiveText.text + "</s>";
            checkImage.enabled = true;
        }
    }

    public void SetObjectiveToUndone()
    {
        if (parent.gameObject.activeSelf)
        {
            objectiveText.text = "</s>" + objectiveText.text;
            checkImage.enabled = false;
        }
    }
}
