using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public abstract class TextDialogue : SerializedScriptableObject
{
    public string Title => title;
    [Space, SerializeField] private string title;

    public Sprite Behavior => behavior;
    [Space, SerializeField] private Sprite behavior;
    public string[] Textsen => textsen;
    [SerializeField, TextArea(5, 50)] private string[] textsen;

    public string[] Textsfr => textsfr;
    [SerializeField, TextArea(5, 50)] private string[] textsfr;

    public string Id => id;
    [Space, SerializeField] private string id;
}