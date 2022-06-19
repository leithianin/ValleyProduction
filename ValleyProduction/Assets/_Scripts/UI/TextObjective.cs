using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public abstract class TextObjective : SerializedScriptableObject
{
    public string Title => title;
    [Space, SerializeField] private string title;

    public string Titlefr => titlefr;
    [Space, SerializeField] private string titlefr;

    public string Texten => texten;
    [SerializeField, TextArea(5, 50)] private string texten;

    public string Textfr => textfr;
    [SerializeField, TextArea(5, 50)] private string textfr;

    public string Id => id;
    [Space, SerializeField] private string id;
}
