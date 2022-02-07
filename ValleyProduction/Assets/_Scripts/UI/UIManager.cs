using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : VLY_Singleton<UIManager>
{
    public List<GameObject> pathButtonList = new List<GameObject>();
    public ChangeRoadInfo RoadInfo;
    public Camera sceneCamera;

    public bool OnMenuOption = false;

    [Header("Visitors Informations")]
    public TouristType hikersInfo;
    public TouristType touristInfo;

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
        foreach(GameObject go in instance.pathButtonList)
        {
            go.SetActive(false);
        }

        foreach(PathData pd in PathManager.instance.pathDataList)
        {
            if(pd.ContainsPoint(pathpoint))
            {
                for(int i=0; i < instance.pathButtonList.Count-1; i++)
                {
                    if(!instance.pathButtonList[i].activeSelf)
                    {
                        instance.pathButtonList[i].GetComponent<ButtonPathData>().pathData = pd;
                        instance.pathButtonList[i].transform.GetChild(0).GetComponent<Text>().text = pd.name;
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

        //Il faut avoir des valeurs fixes et les get selon la difficult�
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

        float offsetPosY = positionButtons.y + 40f;
        float offsetPosX = positionButtons.x + 10f;

        Vector3 offsetPos = new Vector3(offsetPosX, offsetPosY, positionButtons.z);

        Vector2 canvasPos;
        Vector2 screenPoint = instance.sceneCamera.WorldToScreenPoint(offsetPos);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(instance.pathButtonList[0].transform.parent.parent.GetComponent<RectTransform>(), screenPoint, null, out canvasPos);

        instance.pathButtonList[0].transform.parent.transform.localPosition = canvasPos;
    }

    public static void ChangeMenuOptionBool()
    {
        instance.OnMenuOption = !instance.OnMenuOption;
    }

    #region Info Visitors
    public static void InteractWithVisitors(GameObject touchedObject)
    {
        CPN_Informations visitorInfo = touchedObject.GetComponent<CPN_Informations>();
        if (visitorInfo != null)
        {
            ShowInfoVisitor(visitorInfo);
        }
        else
        {
            HideInfoVisitor();
        }
    }

    //Show les informations des visiteurs on click
    public static void ShowInfoVisitor(CPN_Informations cpn_Inf)
    {
        HideInfoVisitor();

       switch (cpn_Inf.visitorType)
        {
            case TypeVisitor.Hiker:
                if (OnBoardingManager.firstClickVisitors)
                {
                    OnBoardingManager.ShowHikerProfileIntro();
                    OnBoardingManager.firstClickVisitors = false;
                }
                ChangeInfoVisitor(instance.hikersInfo, cpn_Inf);
                instance.hikersInfo.gameObject.SetActive(true);
                break;
            case TypeVisitor.Tourist:
                ChangeInfoVisitor(instance.touristInfo, cpn_Inf);
                instance.touristInfo.gameObject.SetActive(true);
                break;
        }
    }

    public static void HideInfoVisitor()
    {
        instance.hikersInfo.gameObject.SetActive(false);
        instance.touristInfo.gameObject.SetActive(false);
    }

    public static void ChangeInfoVisitor(TouristType UI_visitorsInfo, CPN_Informations cpn_Inf)
    {
        UI_visitorsInfo.name.text = cpn_Inf.GetName;
    }

    
    #endregion
}
