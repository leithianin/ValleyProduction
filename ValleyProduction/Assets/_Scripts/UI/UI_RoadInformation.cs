using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_RoadInformation : MonoBehaviour
{
    private PathData pathData;

    [SerializeField] private List<UI_Roads> roadsList = new List<UI_Roads>();
    
    [SerializeField] private TMP_InputField inputField;
    public static bool isEditName = false;

    public UI_RoadInformation ShowInfoRoad(IST_PathPoint pathPoint)
    {
        HideRoadInfo();
        List<PathData> pathDataList = PathManager.GetAllPathDatas(pathPoint);

        for(int i = 0; i < pathDataList.Count; i++)
        {
            roadsList[i].gameObject.SetActive(true);
            roadsList[i].UpdateName(pathDataList[i].name);
        }
        
        OnBoardingManager.onClickPath?.Invoke(true);

        gameObject.SetActive(true);
        return this;
    }

    public void HideRoadInfo()
    {
        foreach(UI_Roads road in roadsList)
        {
            road.gameObject.SetActive(false);
        }
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

    #region InputField
    public void InputFieldOnEnd(string str)
    {
        if (!(str == string.Empty))
        {
            pathData.name = str;
            UpdateInfoRoad();
        }

        //title.gameObject.SetActive(true);
        inputField.gameObject.SetActive(false);
        inputField.text = string.Empty;
        VLY_ContextManager.ChangeContext(0);

        TimerManager.CreateRealTimer(0.1f, () => isEditName = false);
    }

    /// <summary>
    /// Click on Edit Path Name
    /// </summary>
    public void InputFieldOnStart()
    {
        //Désactiver Input Keyboard
        //title.gameObject.SetActive(false);
        inputField.gameObject.SetActive(true);
        inputField.ActivateInputField();
        VLY_ContextManager.ChangeContext(2);
        isEditName = true;
    }

    #endregion
}
