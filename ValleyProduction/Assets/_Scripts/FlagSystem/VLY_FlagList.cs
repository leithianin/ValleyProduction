using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Flag List", menuName ="Create Flag List")]
public class VLY_FlagList : ScriptableObject
{
    [SerializeField] private List<string> flags;
    public List<string> Flags => flags;
}
