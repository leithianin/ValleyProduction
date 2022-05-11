using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_RoadInformation : MonoBehaviour
{
    private PathData pathData;

    [SerializeField] private List<UI_Roads> roadsList = new List<UI_Roads>();
    public static bool isEditName = false;

    public UI_RoadInformation ShowInfoRoad(IST_PathPoint pathPoint)
    {
        ResetRoadInfo();
        List<PathData> pathDataList = PathManager.GetAllPathDatas(pathPoint);

        for(int i = 0; i < pathDataList.Count; i++)
        {
            roadsList[i].gameObject.SetActive(true);
            roadsList[i].UpdateData(pathDataList[i]);
        }
        
        OnBoardingManager.onClickPath?.Invoke(true);

        gameObject.SetActive(true);
        return this;
    }

    public void HideRoadInfo()
    {
        UIManager.HideShownGameObject();
    }

    public void ResetRoadInfo()
    {
        foreach(UI_Roads road in roadsList)
        {
            road.gameObject.SetActive(false);
        }
    }

    public void SetIsEditName(bool boolean)
    {
        isEditName = boolean;
    }

    public void UpdateInfoRoad()
    {
/*
        title.text = pathData.name;
        colorRoad.color = pathData.color;

        stamina.text = (1).ToString();                                       //A FAIRE : Valeur de stamina du chemin
        gaugeStamina.fillAmount = 1f;                                           //A FAIRE : Valeur de stamina du chemin
*/
    }

    public void DeletePath()
    {
        OnBoardingManager.onDestroyPath?.Invoke(true);
        PathManager.DeleteFullPath(pathData);
        UIManager.HideShownGameObject();
    } 
}
