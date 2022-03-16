using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class UIManager : VLY_Singleton<UIManager>
{
    public List<GameObject> pathButtonList = new List<GameObject>();
    public ChangeRoadInfo RoadInfo;
    public Camera sceneCamera;

    [Header("Menu Option")]
    public bool OnMenuOption = false;
    public Button ResumeButton;

    [Header("Settings Menu")]
    [SerializeField] private UnityEvent OnOpenSettings;
    [SerializeField] private UnityEvent OnCloseSettings;

    [Header("Visitors Informations")]
    public TouristType hikersInfo;
    public TouristType touristInfo;
    public TMP_Text nbVisitors;

    [Header("Datas display")]
    [SerializeField] private TMP_Text ressourceCounter;
    [SerializeField] private TMP_Text attractivityCounter;

    [Header("Infrastructure Informations")]
    public GameObject infrastructureInfo;                                             //Pour le moment pas de fenêtre différente selon les infra

    [Header("Tooltips")]
    public Tooltip tooltip;

    [Header("Goals")]
    public TextsDictionary textsScriptable;

    private static Component[] componentTab;
    private static GameObject gameObjectShown;
    public static Tooltip GetTooltip => instance.tooltip;
    public static bool GetIsOnMenuOption => instance.OnMenuOption;
    public static UIManager UIinstance => instance;

    public void SelectStructure(InfrastructurePreview structure)
    {
        switch(structure.RealInfrastructure.StructureType)
        {
            case InfrastructureType.Path:
                if (PathManager.IsOnCreatePath)
                {
                    PathManager.CreatePathData();
                }
                break;
        }

        ConstructionManager.SelectInfrastructureType(structure);
    }

    //Use in Path Button On Click()
    public void OnToolCreatePath(int i)
    {
        ConstructionManager.SelectInfrastructureType(null);

        if (i != 0 && InfrastructureManager.GetCurrentTool != (ToolType)i)
        {
            InfrastructureManager.instance.toolSelected = (ToolType)i;
        }
        else
        {
            InfrastructureManager.instance.toolSelected = ToolType.None;
        }

        switch (InfrastructureManager.GetCurrentTool)
        {
            case ToolType.Place:
                OnBoardingManager.OnClickBuild?.Invoke(true);
                break;
            case ToolType.Move:
                OnBoardingManager.OnClickModify?.Invoke(true);
                break;
            case ToolType.Delete:
                break;
        }
    }

    public void UnselectTool()
    {
        InfrastructureManager.instance.toolSelected = ToolType.None;

        if (PathManager.IsOnCreatePath)
        {
            PathManager.CreatePathData();
        }

        ConstructionManager.SelectInfrastructureType(null);
    }

    //Use in Construction Button On Click()
    public void OnToolCreateConstruction()
    {
        //Create a construction
    }

    #region Menu Option
    public static void ChangeMenuOptionBool()
    {
        instance.OnMenuOption = !instance.OnMenuOption;
    }
    
    public static void HideMenuOption()
    {
        instance.ResumeButton.onClick?.Invoke();
    }
    #endregion

    public void OpenSettingsMenu()
    {
        OnOpenSettings?.Invoke();
    }

    public void CloseSettingsMenu()
    {
        OnCloseSettings?.Invoke();
    }

    #region Interaction/Hide
    public static void InteractWithObject(GameObject touchedObject)
    {
        componentTab = touchedObject.GetComponents(typeof(Component));

        HideShownGameObject();

        foreach (Component component in componentTab)
        {
            switch(component)
            {
                case AU_Informations au_informations:
                    InteractWithInfrastructure(au_informations);
                    break;
                case CPN_Informations cpn_information:
                    InteractWithVisitor(cpn_information);
                    break;
            }
        }
    }
    public static void HideShownGameObject()
    {
        if (gameObjectShown != null)
        {
            OnBoardingManager.OnDeselectInfrastructure?.Invoke(true);
            gameObjectShown.SetActive(false);
        }
    }
    #endregion

    #region Path Info
    public static void ShowRoadsInfos(PathData pathdata)
    {
        instance.RoadInfo.pathData = pathdata;
        instance.RoadInfo.UpdateTitle(pathdata.name);
        instance.RoadInfo.UpdateColor(pathdata.color);
        instance.RoadInfo.UpdateStamina(pathdata.difficulty);

        //Il faut avoir des valeurs fixes et les get selon la difficult�
        instance.RoadInfo.UpdateGaugeStamina(1);

        OnBoardingManager.onClickPath?.Invoke(true);
        instance.RoadInfo.gameObject.SetActive(true);
    }

    public static void ConfirmDelete(PathData pathdata)
    {
        //Show UI
    }

    public static void DeletePath(PathData pathData)
    {
        OnBoardingManager.onDestroyPath?.Invoke(true);
        HideRoadsInfo();
        PathManager.DeleteFullPath(pathData);
    }

    public static void HideRoadsInfo()
    {
        instance.RoadInfo.gameObject.SetActive(false);
    }

    //Buttons Offset de modifier un chemin 
    public static void ButtonsOffset(GameObject pathpoint)
    {
        Vector3 positionButtons = pathpoint.transform.position;

        float offsetPosY = positionButtons.y;
        float offsetPosX = positionButtons.x;

        Vector3 offsetPos = new Vector3(offsetPosX, offsetPosY, positionButtons.z);

        Vector2 canvasPos;
        Vector2 screenPoint = instance.sceneCamera.WorldToScreenPoint(offsetPos);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(instance.pathButtonList[0].transform.parent.parent.GetComponent<RectTransform>(), screenPoint, null, out canvasPos);

        instance.pathButtonList[0].transform.parent.transform.localPosition = canvasPos;
    }

    //Range les PathData dans la liste de boutons 
    public static void ArrangePathButton(IST_PathPoint pathpoint)
    {
        foreach (GameObject go in instance.pathButtonList)
        {
            go.SetActive(false);
        }

        foreach (PathData pd in PathManager.instance.pathDataList)
        {
            if (pd.ContainsPoint(pathpoint))
            {
                for (int i = 0; i < instance.pathButtonList.Count - 1; i++)
                {
                    if (!instance.pathButtonList[i].activeSelf)
                    {
                        instance.pathButtonList[i].GetComponent<ButtonPathData>().pathData = pd;
                        instance.pathButtonList[i].GetComponent<ButtonPathData>().buttonPathpoint = pathpoint;
                        instance.pathButtonList[i].transform.GetChild(0).GetComponent<Text>().text = pd.name;
                        instance.pathButtonList[i].SetActive(true);
                        break;
                    }
                }
            }
        }

        ButtonsOffset(pathpoint.gameObject);
    }

    //Clique sur un des boutons
    public static void ChooseButton(ButtonPathData buttonPath)
    {
        foreach (GameObject go in instance.pathButtonList)
        {
            go.SetActive(false);
        }

        switch (InfrastructureManager.GetCurrentTool)
        {
            case ToolType.None:
                ShowRoadsInfos(buttonPath.pathData);
                break;
            case ToolType.Delete:
                buttonPath.buttonPathpoint.Remove(buttonPath.pathData);
                break;
        }
    }
    #endregion

    #region Info Visitors
    public static void UpdateNbVisitors(int nb)
    {
        instance.nbVisitors.text = nb.ToString();
    }

    //Show les informations des visiteurs on click
    public static void InteractWithVisitor(CPN_Informations visitorInfo)
    {    
        ShowInfoVisitor(visitorInfo);      
    }
    public static void ShowInfoVisitor(CPN_Informations cpn_Inf)
    {
       OnBoardingManager.OnClickVisitorEco?.Invoke(true);
       switch (cpn_Inf.visitorType)
        {
            case TypeVisitor.Hiker:
                if (OnBoardingManager.firstClickVisitors)
                {
                    OnBoardingManager.OnClickVisitorPath?.Invoke(true);
                    OnBoardingManager.ShowHikerProfileIntro();
                    OnBoardingManager.firstClickVisitors = false;
                }
                ChangeInfoVisitor(instance.hikersInfo, cpn_Inf);
                instance.hikersInfo.gameObject.SetActive(true);
                gameObjectShown = instance.hikersInfo.gameObject;
                break;
            case TypeVisitor.Tourist:
                ChangeInfoVisitor(instance.touristInfo, cpn_Inf);
                instance.touristInfo.gameObject.SetActive(true);
                gameObjectShown = instance.touristInfo.gameObject;
                break;
        }
    }
    public static void ChangeInfoVisitor(TouristType UI_visitorsInfo, CPN_Informations cpn_Inf)
    {
        UI_visitorsInfo.name.text = cpn_Inf.GetName;
    }
    #endregion

    #region Info Infrastructure
    //Show les informations des visiteurs on click
    public static void InteractWithInfrastructure(AU_Informations infoInfra)
    {
        ShowInfoInfrastructure(infoInfra);
    }

    public static void ShowInfoInfrastructure(AU_Informations AU_Inf)
    {
        OnBoardingManager.OnClickInfrastructure?.Invoke(true);
        instance.infrastructureInfo.SetActive(true);
        gameObjectShown = instance.infrastructureInfo;
        //Show UI with info
    }
    #endregion

    #region Ressources
    public void UpdateRessourceCount(int ressource)
    {
        ressourceCounter.text = ressource.ToString();
    }

    public void UpdateAttractivityCount(float attractivity)
    {
        attractivityCounter.text = attractivity.ToString("F1");
    }
    #endregion
}
