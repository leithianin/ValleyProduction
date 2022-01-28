using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class IFB_ChangeFontStyle : MonoBehaviour, IFeedbackPlayer
{
    public TMP_Text text;
    private FontStyles font;
    
    public void Play()
    {
        text.fontStyle = font;
    }

    public void Play(int i)
    {
        FontStyles currentFont = (FontStyles)i;
        switch (currentFont)
        {
            case FontStyles.Normal:                                                                 // 0
                font = FontStyles.Normal;
                break;

            case FontStyles.Strikethrough:                                                          // 64
                font = FontStyles.Strikethrough;
                break;

            case FontStyles.Underline:                                                              //4
                font = FontStyles.Underline;
                break;
        }

        Play();
    }
}
