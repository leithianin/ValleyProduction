using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Name", menuName = "Name/Create NameList")]
public class NameScriptable : ScriptableObject
{
    public List<string> nameList = new List<string>();
}
