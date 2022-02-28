using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextHolder : MonoBehaviour
{
    public TextBase Value => value;
    [SerializeField] private TextBase value;
}
