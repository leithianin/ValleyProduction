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

    //Range les PathData dans la liste de boutons 
    public static void ArrangePathButton(IST_PathPoint pathpoint)
    {

        foreach(PathData pd in PathManager.instance.pathDataList)
        {
            if(pd.ContainsPoint(pathpoint))
            {
                for(int i=0; i < instance.pathButtonList.Count-1; i++)
                {
                    if(!instance.pathButtonList[i].activeSelf)
                    {
                        instance.pathButtonList[i].GetComponent<ButtonPathData>().pathData = pd;
                        instance.pathButtonList[i].SetActive(true);
                        break;
                    }
                }
            }
        }
    }

    public static void ChooseButton(ButtonPathData buttonPath)
    {
        foreach(GameObject go in instance.pathButtonList)
        {
            go.SetActive(false);
        }

        PathManager.SelectPathWithPathData(buttonPath.pathData);
    }
}
