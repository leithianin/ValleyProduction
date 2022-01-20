using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : VLY_Singleton<UIManager>
{
    public List<GameObject> pathButtonList = new List<GameObject>();
    public ChangeRoadInfo RoadInfo;
    public Camera sceneCamera;

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

        ButtonsOffset(pathpoint.gameObject);
    }

    public static void ShowRoadsInfos(PathData pathdata)
    {
        instance.RoadInfo.UpdateTitle(pathdata.name);
        instance.RoadInfo.UpdateColor(pathdata.color);
        instance.RoadInfo.UpdateStamina(pathdata.difficulty);

        //Il faut avoir des valeurs fixes et les get selon la difficulté
        instance.RoadInfo.UpdateGaugeStamina(1);

        instance.RoadInfo.gameObject.SetActive(true);
    }

    public static void HideRoadsInfo()
    {
        instance.RoadInfo.gameObject.SetActive(false);
    }

    //Clique sur un des boutons
    public static void ChooseButton(ButtonPathData buttonPath)
    {
        foreach(GameObject go in instance.pathButtonList)
        {
            go.SetActive(false);
        }

        PathManager.SelectPathWithPathData(buttonPath.pathData);
    }

    //Buttons Offset de modifier un chemin 
    public static void ButtonsOffset(GameObject pathpoint)
    {
        Vector3 positionButtons = pathpoint.transform.position;

        float offsetPosY = positionButtons.y + 1.5f;
        float offsetPosX = positionButtons.x + 8f;

        Vector3 offsetPos = new Vector3(offsetPosX, offsetPosY, positionButtons.z);

        Vector2 canvasPos;
        Vector2 screenPoint = instance.sceneCamera.WorldToScreenPoint(offsetPos);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(instance.pathButtonList[0].transform.parent.parent.GetComponent<RectTransform>(), screenPoint, null, out canvasPos);

        instance.pathButtonList[0].transform.parent.transform.localPosition = canvasPos;
    }
}
