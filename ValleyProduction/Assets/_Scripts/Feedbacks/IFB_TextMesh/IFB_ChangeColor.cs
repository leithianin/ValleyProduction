using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IFB_ChangeColor : MonoBehaviour, IFeedbackPlayer
{
    public TMP_Text text;
    private Color currentColor;
    private Color baseColor;
    public Color color;

    public void Start()
    {
        baseColor = text.color;
    }

    public void Play()
    {
        
    }

    public void ChangeColor()
    {
        text.color = color;
    }

    public void RestoreColor()
    {
        text.color = baseColor;
    }
}
