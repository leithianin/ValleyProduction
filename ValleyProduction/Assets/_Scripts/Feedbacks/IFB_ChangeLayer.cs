using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IFB_ChangeLayer : MonoBehaviour, IFeedbackPlayer
{
    private int layerInt;

    public void Play()
    {
        gameObject.layer = layerInt;
    }

    public void PlayInt(int i)
    {
        layerInt = i;
        Play();
    }
}
