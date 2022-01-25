using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeAlpha : MonoBehaviour, IFeedbackPlayer
{
    public Image image;
    [SerializeField] private float alpha;

    public void Play()
    {
        Debug.Log("Change Alpha");
        image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
    }

    public void Play(float integer)
    {
        alpha = integer;
        Play();
    }
}
