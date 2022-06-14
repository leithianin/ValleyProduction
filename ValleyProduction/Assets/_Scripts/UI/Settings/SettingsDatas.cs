using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Language { fr, en};
[CreateAssetMenu(fileName ="Settings Data", menuName ="Valley/UI/Settings Data")]
public class SettingsDatas : ScriptableObject
{
    public bool cameraEdgeScrollingActive = true;
    public bool cameraMouseScrollingActive = true;
    public bool cameraInvertHorizontalWheelRotation = false;
    public bool cameraInvertVerticalWheelRotation = false;

    public Language lang = Language.en;
}
