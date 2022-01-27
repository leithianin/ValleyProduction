using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : VLY_Singleton<MenuManager>
{
    //Voyage entre la scene Menu et la scene Game

    void doExitGame()
    {
        Application.Quit();
    }
}
