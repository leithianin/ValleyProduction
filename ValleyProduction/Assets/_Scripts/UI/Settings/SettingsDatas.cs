using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Settings Data", menuName ="UI/Settings Data")]
public class SettingsDatas : ScriptableObject
{
    public bool cameraEdgeScrollingActive = true;
    public bool cameraMouseScrollingActive = true;
    public bool cameraInvertHorizontalWheelRotation = false;
    public bool cameraInvertVerticalWheelRotation = false;
}
