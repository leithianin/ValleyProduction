using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IFB_ChangeHeight : MonoBehaviour, IFeedbackPlayer
{
    private int newHeight;
    public RectTransform rt;

    public void Play()
    {
        var bgSize = new Vector2(rt.sizeDelta.x, newHeight);
        rt.sizeDelta = bgSize;

        //Canvas.ForceUpdateCanvases();
    }

    public void Play(int i)
    {
        newHeight = i;
        Play();
    }
}
