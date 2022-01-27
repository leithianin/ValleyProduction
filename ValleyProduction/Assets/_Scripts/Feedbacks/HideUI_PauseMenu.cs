using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideUI_PauseMenu : MonoBehaviour
{
    public void HideMenu()
    {
        Debug.Log("Test");
        gameObject.SetActive(false);
    }
}
