using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public abstract class TextBase : SerializedScriptableObject
{
    /*public TextCategory Category => category;
    [SerializeField] private TextCategory category;*/
    public string Title => title;
    [Space, SerializeField] private string title;

    public string[] Texts => texts;
    [SerializeField, TextArea(5, 50)] private string[] texts;

    public string Id => id;
    [Space, SerializeField] private string id;
}
