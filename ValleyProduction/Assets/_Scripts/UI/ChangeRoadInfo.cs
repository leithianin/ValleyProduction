using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChangeRoadInfo : MonoBehaviour
{
    public PathData pathData;

    public TMP_Text title;
    public TMP_Text stamina;
    public Image colorRoad;
    public Image gaugeStamina;

    public void UpdateTitle(string text)
    {
        title.text = text;
    }

    public void UpdateStamina(float staminaNb)
    {
        stamina.text = staminaNb.ToString();
    }

    public void UpdateColor(Color color)
    {
        colorRoad.color = color;
    }

    public void UpdateGaugeStamina(float staminaNb)
    {
        gaugeStamina.fillAmount = staminaNb;
    }

    public void DeletePath()
    {
        UIManager.DeletePath(pathData);
    }
}
