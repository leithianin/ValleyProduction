using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sound Data", menuName = "Valley/Create New Sound Data")]
public class SoundDatas : ScriptableObject
{
    public float masterVolume;
    public float musicVolume;
    public float sfxVolume;
    public float uiVolume;
    public float ambientVolume;
}
