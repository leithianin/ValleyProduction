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
    public static bool isEditName = false;

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
        if (!(str == string.Empty))
        {
            pathData.name = str;
            UpdateInfoRoad();
        }

        title.gameObject.SetActive(true);
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
        title.gameObject.SetActive(false);
        inputField.gameObject.SetActive(true);
        inputField.ActivateInputField();
        VLY_ContextManager.ChangeContext(2);
        isEditName = true;
    }

    #endregion
}
