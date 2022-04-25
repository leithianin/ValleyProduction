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

    [SerializeField] private TMP_InputField inputField;

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
        title.text = pathData.name;
        colorRoad.color = pathData.color;

        stamina.text = (1).ToString();                                       //A FAIRE : Valeur de stamina du chemin
        gaugeStamina.fillAmount = 1f;                                           //A FAIRE : Valeur de stamina du chemin
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
        pathData.name = str;
        UpdateInfoRoad();
        title.gameObject.SetActive(true);
        inputField.gameObject.SetActive(false);
    }

    /// <summary>
    /// Click on Edit Path Name
    /// </summary>
    public void InputFieldOnStart()
    {
        title.gameObject.SetActive(false);
        inputField.gameObject.SetActive(true);
        inputField.ActivateInputField();
    }


    #endregion
}
