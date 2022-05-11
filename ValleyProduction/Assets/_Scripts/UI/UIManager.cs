using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class UIManager : VLY_Singleton<UIManager>
{
    [Header("Tool Information")]
    public UI_Tool toolInfo;

    [Header("Menu Option")]
    public UI_PauseMenu pauseMenuInfo;

    [Header("Road Informations")]
    public UI_RoadInformation RoadInfo;
    public static UI_RoadInformation GetRoadInfo => instance.RoadInfo;

    [Header("Visitors Informations")]
    public UI_VisitorInformation visitorInfo;
    public static UI_VisitorInformation GetVisitorInformation => instance.visitorInfo;

    [Header("Infrastructure Informations")]
    public UI_InfrastructureInformation infrastructureInfo;                                             //Pour le moment pas de fenêtre différente selon les infra
    public static UI_InfrastructureInformation GetInfrastructureInfo => instance.infrastructureInfo;

    [Header("Datas display")]
    public UI_DataDisplay dataInfo;

    [Header("Choose Path Buttons")]
    public UI_ChoosePathButtons choosePathButtonInfo;

    [Header("Tooltips")]
    public Tooltip tooltip;

    [Header("Goals")]
    public TextsDictionary textsScriptable;

    [Header("Quest")]
    [SerializeField] private UI_QuestDisplayer questDisplayer;

    private static Component[] componentTab;
    private static GameObject gameObjectShown;
    public static Tooltip GetTooltip => instance.tooltip;

    #region Interaction/Hide
    public static void InteractWithObject(GameObject touchedObject)
    {
        componentTab = touchedObject.GetComponents(typeof(Component));

        HideShownGameObject();

        foreach (Component component in componentTab)
        {
            switch (component)
            {
                case IST_PathPoint ist_pathpoint:
                    InteractWithRoad(ist_pathpoint);
                    break;
                case VisitorBehavior visitorBehavior:
                    InteractWithVisitor(visitorBehavior.GetComponent<CPN_Informations>());
                    break;
            }
        }
    }

    public static void HideShownGameObject()
    {
        if (gameObjectShown != null && !UI_RoadInformation.isEditName)
        {
            GetInfrastructureInfo.ResetSavedInfrastructe();
            GetVisitorInformation.ResetSavedVisitors();
            OnBoardingManager.onHideVisitorInfo?.Invoke(true);
            OnBoardingManager.OnDeselectInfrastructure?.Invoke(true);
            gameObjectShown.SetActive(false);
        }
    }
    #endregion

    #region Tool
    public static void OnToolCreatePath(int i)
    {
        instance.ToolCreatePath(i);
    }

    public void ToolCreatePath(int i)
    {
        toolInfo.OnToolCreatePath(i);
    }

    public static void UnlockStructure(InfrastructurePreview toUnlock)
    {
        instance.toolInfo.UnlockStructure(toUnlock);
    }
    #endregion

    #region Pause Menu
    public static bool IsOnMenuBool()
    {
        if (instance.pauseMenuInfo != null)
        {
            return instance.pauseMenuInfo.OnMenuOption;
        }

        return false;   
    }

    public static void HideMenuOption()
    {
        instance.pauseMenuInfo.HideMenuOption();
    }
    #endregion

    #region Path Info
    public static void InteractWithRoad(IST_PathPoint pathPoint)
    {
        instance.ShowInfoRoad(pathPoint);
    }

    public void ShowInfoRoad(IST_PathPoint pathPoint)
    {
        gameObjectShown = RoadInfo.ShowInfoRoad(pathPoint).gameObject;
    }
    #endregion

    #region ChoosePathButton
    //Range les PathData dans la liste de boutons 
    public static void ArrangePathButton(IST_PathPoint pathpoint)
    {
        instance.CallArrangePathButton(pathpoint);
    }

    public void CallArrangePathButton(IST_PathPoint pathpoint)
    {
        choosePathButtonInfo.ArrangePathButton(pathpoint);
    }

    //Clique sur un des boutons chemin
    public static void ChooseButton(ButtonPathData buttonPath)
    {
        gameObjectShown = null;

        instance.choosePathButtonInfo.HidePathButton();

        //Selon l'outil selectionné 
        switch (InfrastructureManager.GetCurrentTool)
        {
            case ToolType.None:
                //InteractWithRoad(buttonPath.pathData);
                break;
            case ToolType.Delete:
                buttonPath.buttonPathpoint.Remove(buttonPath.pathData);
                break;
        }
    }
    #endregion

    #region Info Visitors
    //Show les informations des visiteurs on click
    public static void InteractWithVisitor(CPN_Informations cpn_Inf)
    {
        HideShownGameObject();
        instance.ShowInfoVisitor(cpn_Inf);
    }

    public void ShowInfoVisitor(CPN_Informations cpn_Inf)
    {
        gameObjectShown = visitorInfo.ShowInfoVisitor(cpn_Inf).gameObject;
    }

    public static void UpdateCurrentNoise()
    {
        if(GetVisitorInformation.currentTourist != null)
        {
            GetVisitorInformation.UpdateNoise();
        }
    }

    public static void UpdateCurrentPollution()
    {
        if (GetVisitorInformation.currentTourist != null)
        {
            GetVisitorInformation.UpdatePollution();
        }
    }

    public static void UpdateCurrentStamina()
    {
        if (GetVisitorInformation.currentTourist != null)
        {
            GetVisitorInformation.UpdateStamina();
        }
    }

    public static void OnUnsetVisitor(VisitorScriptable visitor)
    {
        if(GetVisitorInformation != null && GetVisitorInformation.currentInfo != null && visitor == GetVisitorInformation.currentInfo.scriptable)
        {
            HideShownGameObject();
        }
    }

    #endregion

    #region Info Infrastructure
    //Show les informations des visiteurs on click
    public static void InteractWithInfrastructure(ECO_AGT_Informations infoInfra, Infrastructure baseStruct)
    {
        HideShownGameObject();
        instance.ShowInfoInfrastructure(infoInfra, baseStruct);
    }

    public void ShowInfoInfrastructure(ECO_AGT_Informations infoInfra, Infrastructure baseStruct)
    {
        OnBoardingManager.OnClickInfrastructure?.Invoke(true);
        infrastructureInfo.ShowStructureInformation(infoInfra, baseStruct);
        gameObjectShown = infrastructureInfo.gameObject;
    }

    /// <summary>
    /// Update current NB Visitors each time we add or remove 1 visitors
    /// </summary>
    public static void UpdateCurrentNbVisitors()
    {
        if (GetInfrastructureInfo.openedInfrastructure != null)
        {
            GetInfrastructureInfo.UpdateCurrentNbInfo(GetInfrastructureInfo.openedInfrastructure);
        }
    }

    /// <summary>
    /// Update le nombre total de visiteurs sur l'UI actuellement affiché
    /// </summary>
    public static void UpdateTotalNbVisitors()
    {
        if (GetInfrastructureInfo.openedInfrastructure != null)
        {
            GetInfrastructureInfo.UpdateTotalNbInfo(GetInfrastructureInfo.openedInfrastructure);
        }
    }

    public static void UpdateTotalMoney()
    {
        if (GetInfrastructureInfo.openedInfrastructure != null)
        {
            GetInfrastructureInfo.UpdateTotalMoney(GetInfrastructureInfo.openedInfrastructure);
        }
    }
    #endregion

    #region Quests
    public static void UpdateQuestStatus(VLY_Quest updatedQuest)
    {
        switch(updatedQuest.state)
        {
            case QuestObjectiveState.PendingCompletion:
                instance.questDisplayer.SetPendingCompletion(updatedQuest);
                break;
            case QuestObjectiveState.Started:
                instance.questDisplayer.SetQuestObjective(updatedQuest, updatedQuest.GetCurrentStageObjectives());
                break;
        }
    }

    public static void ShowQuestRewards(List<QST_Reward> rewards)
    {
        instance.questDisplayer.SetRewardToDisplay(rewards);
    }
    #endregion

}
