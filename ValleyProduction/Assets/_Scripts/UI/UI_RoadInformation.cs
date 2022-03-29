using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_RoadInformation : MonoBehaviour
{
    private PathData pathData;

    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text stamina;
    [SerializeField] private Image colorRoad;
    [SerializeField] private Image gaugeStamina;

    public UI_RoadInformation ShowInfoRoad(PathData pd)
    {
        pathData = pd;
        OnBoardingManager.onClickPath?.Invoke(true);
        UpdateInfoRoad();

        gameObject.SetActive(true);
        return this;
    }

    public void UpdateInfoRoad()
    {
        title.text      = pathData.name ;
        colorRoad.color = pathData.color;

        stamina.text    = (1).ToString();                                       //A FAIRE : Valeur de stamina du chemin
        gaugeStamina.fillAmount = 1f;                                           //A FAIRE : Valeur de stamina du chemin
    }

    public void DeletePath()
    {
        OnBoardingManager.onDestroyPath?.Invoke(true);
        PathManager.DeleteFullPath(pathData);
        UIManager.HideShownGameObject();
    }
}
