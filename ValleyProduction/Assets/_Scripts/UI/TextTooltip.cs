using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public abstract class TextTooltip : SerializedScriptableObject
{
    public string Titleen => titleen;
    [Space, SerializeField] private string titleen;

    public string Titlefr => titlefr;
    [Space, SerializeField] private string titlefr;

    public string Texten => texten;
    [SerializeField, TextArea(5, 50)] private string texten;

    public string Textfr => textfr;
    [SerializeField, TextArea(5, 50)] private string textfr;
}
