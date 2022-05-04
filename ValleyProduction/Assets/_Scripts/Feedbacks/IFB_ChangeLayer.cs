using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IFB_ChangeLayer : MonoBehaviour, IFeedbackPlayer
{
    private int layerInt;
    public List<GameObject> meshGameObject;

    public void Play()
    {
        foreach (GameObject go in meshGameObject)
        {
            go.layer = layerInt;
        }
    }

    public void PlayInt(int i)
    {
        layerInt = i;
        Play();
    }

    public void PlayString(string layerName)
    {
        layerInt = LayerMask.NameToLayer(layerName);
        Play();
    }
}
