using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewName", menuName = "Valley/NameList")]
public class NameScriptable : ScriptableObject
{
    public List<string> nameList = new List<string>();
}
