using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_PauseMenu : MonoBehaviour
{
    public void LoadScene(int index)
    {
        MenuManager.LoadScene(index);
    }

    public void ExitGame()
    {
        MenuManager.Exit();
    }
}
