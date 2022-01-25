using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeAlpha : MonoBehaviour, IFeedbackPlayer
{
    public Image image;
    [SerializeField] private int alpha;

    public void Play()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
    }

    public void Play(int integer)
    {
        alpha = integer;
        Play();
    }
}
