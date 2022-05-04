using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Roads : MonoBehaviour
{
    private PathData pathData;
    [SerializeField] private TextMeshProUGUI nameText;

    [Header("InputField")]
    [SerializeField] private TMP_InputField inputField;

    /// <summary>
    /// Save PathData and change UI Name
    /// </summary>
    /// <param name="pd"></param>
    public void UpdateData(PathData pd)
    {
        pathData = pd;
        nameText.text = pd.name;
    }

    public void UpdateNameText()
    {
        nameText.text = pathData.name;
    }

    public void ChangePathName()
    {

    }

    #region InputField
    public void InputFieldOnEnd(string str)
    {
        if (!(str == string.Empty))
        {
            pathData.name = str;
            UpdateNameText();
        }

        inputField.gameObject.SetActive(false);
        nameText.gameObject.SetActive(true);
        inputField.text = string.Empty;
        VLY_ContextManager.ChangeContext(0);

        TimerManager.CreateRealTimer(0.1f, () => UIManager.GetRoadInfo.SetIsEditName(false));
    }

    /// <summary>
    /// Click on Edit Path Name
    /// </summary>
    public void InputFieldOnStart()
    {
        //Désactiver Input Keyboard
        nameText.gameObject.SetActive(false);
        inputField.gameObject.SetActive(true);
        inputField.ActivateInputField();
        VLY_ContextManager.ChangeContext(2);
        UIManager.GetRoadInfo.SetIsEditName(true);
    }

    #endregion
}
