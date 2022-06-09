using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShotsSequence", menuName = "Valley/ShotsSequence", order = 1)]
public class ShotsSequence : ScriptableObject
{
    public CameraData[] sequence;
}
