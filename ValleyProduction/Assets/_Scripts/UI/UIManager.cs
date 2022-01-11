using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : VLY_Singleton<UIManager>
{
    private InfrastructureType buttonSelected = InfrastructureType.None;

    public InfrastructureType GetSelectedButton => buttonSelected;

    //Use in Path Button On Click()
    public void OnToolCreatePath()
    {
        if(buttonSelected == InfrastructureType.PathTools)
        {
            buttonSelected = InfrastructureType.None;
        }
        else
        {
            buttonSelected = InfrastructureType.PathTools;
        }
    }

    //Use in Construction Button On Click()
    public void OnToolCreateConstruction()
    {
        //Create a construction
    }
}
