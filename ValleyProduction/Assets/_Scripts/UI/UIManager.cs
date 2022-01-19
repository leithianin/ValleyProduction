using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : VLY_Singleton<UIManager>
{
    public List<GameObject> pathButtonList = new List<GameObject>();

    //Use in Path Button On Click()
    public void OnToolCreatePath()
    {   
        ConstructionManager.SelectInfrastructureType(InfrastructureType.PathTools);      
    }

    //Use in Construction Button On Click()
    public void OnToolCreateConstruction()
    {
        //Create a construction
    }
}
