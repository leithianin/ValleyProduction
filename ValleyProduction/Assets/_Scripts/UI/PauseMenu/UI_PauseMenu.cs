using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class UI_PauseMenu : MonoBehaviour
{
    [Header("Settings Menu")]
    [SerializeField] private UnityEvent OnOpenSettings;
    [SerializeField] private UnityEvent OnCloseSettings;

    [HideInInspector] public bool OnMenuOption = false;
    [SerializeField] private Button resumeButton;


    public void LoadScene(int index)
    {
        MenuManager.LoadScene(index);
    }

    public void ExitGame()
    {
        MenuManager.Exit();
    }

    public void ChangeMenuOptionBool()
    {
        OnMenuOption = !OnMenuOption;
    }
    public void HideMenuOption()
    {
        resumeButton.onClick?.Invoke();
    }

    #region Settings
    public void OpenSettingsMenu()
    {
        OnOpenSettings?.Invoke();
    }

    public void CloseSettingsMenu()
    {
        OnCloseSettings?.Invoke();
    }
    #endregion
}
