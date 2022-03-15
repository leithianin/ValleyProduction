using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private SettingsDatas datas;

    public void SetInvertedHorizontalCamera(bool value)
    {
        datas.cameraInvertHorizontalWheelRotation = value;
    }
    public void SetInvertedVerticalCamera(bool value)
    {
        datas.cameraInvertVerticalWheelRotation = value;
    }

    public void SetScrollEdgeCamera(bool value)
    {
        datas.cameraEdgeScrollingActive = value;
    }

    public void SetScrollMouseCamera(bool value)
    {
        datas.cameraMouseScrollingActive = value;
    }
}
