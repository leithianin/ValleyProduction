using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ButtonEnum { Path, None};
public class UIManager : VLY_Singleton<UIManager>
{
    private ButtonEnum buttonSelected = ButtonEnum.None;

    public ButtonEnum GetSelectedButton => buttonSelected;

    public void OnToolCreatePath()
    {
        if(buttonSelected == ButtonEnum.Path)
        {
            buttonSelected = ButtonEnum.None;
        }
        else
        {
            buttonSelected = ButtonEnum.Path;
        }
    }

    public void OnToolCreateConstruction()
    {
        //Create a construction
    }
}
