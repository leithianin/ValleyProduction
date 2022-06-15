using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        Debug.Log("ScrollEdge : " + value);

        datas.cameraEdgeScrollingActive = value;
    }

    public void SetScrollMouseCamera(bool value)
    {
        datas.cameraMouseScrollingActive = value;
    }

    public void SetLanguage(TMP_Dropdown dropdown)
    {
        Debug.Log(dropdown.value);

        switch(dropdown.value)
        {
            case 0:
                datas.lang = Language.en;
                break;
            case 1:
                datas.lang = Language.fr;
                break;
            default:
                break;
        }

        UIManager.OnLanguageChange?.Invoke();
    }
}
