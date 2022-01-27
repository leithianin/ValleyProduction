using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IFB_ChangeText : MonoBehaviour, IFeedbackPlayer
{
    public TMP_Text text;
    private string stringText = string.Empty;

    public void Play()
    {
        text.text = stringText;
    }

    public void Play(string newText)
    {
        stringText = newText;
        Play();
    }
}
