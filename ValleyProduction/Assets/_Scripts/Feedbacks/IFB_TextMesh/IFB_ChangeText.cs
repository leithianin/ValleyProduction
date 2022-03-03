using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IFB_ChangeText : MonoBehaviour, IFeedbackPlayer
{
    public TMP_Text textComponent;
    private string textString;

    public void Play()
    {
        textComponent.text = textString;
    }

    public void Play(string newText)
    {
        textString = newText;
        Play();
    }
}
