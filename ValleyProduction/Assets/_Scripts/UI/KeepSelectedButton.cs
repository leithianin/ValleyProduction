using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepSelectedButton : MonoBehaviour
{
    public List<Animator> buttonList = new List<Animator>();

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            KeepButtonSelected();
        }
    }

    public void KeepButtonSelected()
    {
        switch (MenuManager.GetSceneToLoad)
        {
            case 1:
                buttonList[0].SetTrigger("Selected");
                break;
            case 2:
                buttonList[1].SetTrigger("Selected");
                break;
            case 3:
                buttonList[2].SetTrigger("Selected");
                break;
        }
    }
}
