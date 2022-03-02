using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IFB_ChangeWidth : MonoBehaviour, IFeedbackPlayer
{
    private int newWidth;
    public RectTransform rt;

    public void Play()
    {
        var bgSize = new Vector2(newWidth, rt.sizeDelta.y);
        rt.sizeDelta = bgSize;

        //Canvas.ForceUpdateCanvases();
    }

    public void Play(int i)
    {
        newWidth = i;
        Play();
    }
}
