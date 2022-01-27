using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IFB_ChangeImage : MonoBehaviour, IFeedbackPlayer
{
    public Image image;
    public Sprite sprite;
    private Sprite baseSprite;

    private void Start()
    {
        baseSprite = GetComponent<Image>().sprite;
    }

    public void Play()
    {
        
    }

    public void ChangeSprite()
    {
        image.sprite = sprite;
    }

    public void RestoreSprite()
    {
        image.sprite = baseSprite;
    }
}
