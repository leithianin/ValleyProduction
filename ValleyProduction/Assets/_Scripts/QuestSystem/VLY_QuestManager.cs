using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VLY_QuestManager : VLY_Singleton<VLY_QuestManager>
{
    [SerializeField] private VLY_Quest[] allQuests;

    //Quest objectives behavior
    private List<QST_ObjectiveBehavior> objectivesBehaviors = new List<QST_ObjectiveBehavior>();

    //Quest rewards behavior
    private List<QST_RewardBehavior> rewardBehaviors = new List<QST_RewardBehavior>();

    [SerializeField] private VLY_Quest startQuest;

    private void Start()
    {
        //Création des différents behavior pour les Objectifs
        objectivesBehaviors.Add(new QST_OBJB_Ressource());
        objectivesBehaviors.Add(new QST_OBJB_PlaceStructure());
        objectivesBehaviors.Add(new QST_OBJB_VisitorReachLandmark());
        objectivesBehaviors.Add(new QST_OBJB_FlagValue());
        objectivesBehaviors.Add(new QST_OBJB_TriggerFlag());
        objectivesBehaviors.Add(new QST_OBJB_ValidPathToLandmark());

        //Création des différents behavior pour les Rewards
        rewardBehaviors.Add(new QST_RWDB_Ressource());
        rewardBehaviors.Add(new QST_RWDB_UnlockStructure());
        rewardBehaviors.Add(new QST_RWDB_VisitorType());
        rewardBehaviors.Add(new QST_RWDB_QuestStart());

        //Récupération des quêtes dans le projet.
        allQuests = Resources.FindObjectsOfTypeAll<VLY_Quest>();

        //Réinitialisaiton des quêtes
        for(int i = 0; i < allQuests.Length; i++)
        {
            allQuests[i].Reset();
        }

        //Placeholder : Démarre la première quête
        TimerManager.CreateRealTimer(2f, () => BeginQuest(startQuest));        
        //BeginQuest(startQuest);
    }

    /// <summary>
    /// Commence la quête si elle n'est pas déjà avancée.
    /// </summary>
    /// <param name="quest">La quête à démarrer</param>
    public static void BeginQuest(VLY_Quest quest)
    {
        if(quest.state == QuestObjectiveState.Pending)
        {
            instance.UpdateQuestObjective(quest);
        }
    }

    /// <summary>
    /// Appelé quand un quête se finit. Fait gagner les récompenses au joueur.
    /// </summary>
    /// <param name="quest">La quête à finir.</param>
    public static void CompleteQuest(VLY_Quest quest)
    {
        quest.state = QuestObjectiveState.Completed;

        for(int i = 0; i < quest.Rewards.Count; i++)
        {
            QST_RewardBehavior behavior = instance.GetRewardBehavior(quest.Rewards[i]);

            behavior.GiveReward(quest.Rewards[i]);

            Debug.Log("Reward given : " + quest.Rewards[i]);
        }

        UIManager.ShowQuestRewards(quest.Rewards);
    }

    /// <summary>
    /// Vérifie l'état de la quête et met à jour le prochain objectif.
    /// </summary>
    /// <param name="updatedQuest">La quête à update.</param>
    private void UpdateQuestObjective(VLY_Quest updatedQuest) //CODE REVIEW : Clean la fonction (Séparer la partie "Quest Stage" dans une autre fonction
    {
        int i = 0;

        for (i = 0; i < updatedQuest.Stages.Count; i++)
        {
            bool hasStartedObjective = false;
            switch(updatedQuest.Stages[i].State)
            {
                case QuestObjectiveState.Completed:
                    continue;
                case QuestObjectiveState.Started:
                    bool isPhaseComplete = true;
                    for(int j = 0; j < updatedQuest.Stages[i].Objectives.Count; j++)
                    {
                        if(updatedQuest.Stages[i].Objectives[j].State != QuestObjectiveState.Completed)
                        {
                            isPhaseComplete = false;
                            break;
                        }
                    }

                    if(isPhaseComplete)
                    {
                        SetStageStatus(updatedQuest.Stages[i], QuestObjectiveState.Completed);
                    }
                    else
                    {
                        hasStartedObjective = true;
                    }
                    break;
                case QuestObjectiveState.Pending:
                    for (int j = 0; j < updatedQuest.Stages[i].Objectives.Count; j++)
                    {
                        SetObjectiveStatus(updatedQuest.Stages[i].Objectives[j], QuestObjectiveState.Started);

                        updatedQuest.Stages[i].Objectives[j].OnUpdateState += (QuestObjectiveState state) => UpdateQuestObjective(updatedQuest);
                    }

                    SetStageStatus(updatedQuest.Stages[i], QuestObjectiveState.Started);

                    if (i == 0)
                    {
                        updatedQuest.state = QuestObjectiveState.Started; //CODE REVIEW : Voir pour le mettre dans une fonction (Gérer les feedbacks)
                    }

                    hasStartedObjective = true;
                    break;
            }

            if(hasStartedObjective)
            {
                break;
            }
        }

        if (i >= updatedQuest.Stages.Count)
        {
            Debug.Log("Complete : " + updatedQuest);
            //TO DO : Mettre à jour l'UI pour afficher le bouton de completion d'une quête
            updatedQuest.state = QuestObjectiveState.PendingCompletion;
            //CompleteQuest(updatedQuest);
        }

        UIManager.UpdateQuestStatus(updatedQuest);
    }

    /// <summary>
    /// Met à jour l'état d'un objectif
    /// </summary>
    /// <param name="objective">L'objectif à mettre à jour.</param>
    /// <param name="state">L'état de l'objectif.</param>
    public static void SetObjectiveStatus(QST_Objective objective, QuestObjectiveState state)
    {
        QST_ObjectiveBehavior usedBehavior = instance.GetObjectiveBehavior(objective);

        usedBehavior.SetObjectiveStatus(objective, state);
    }

    private static void SetStageStatus(QST_ObjectiveStage stage, QuestObjectiveState state)
    {
        if(state > stage.State)
        {
            stage.State = state;

            if(stage.State == QuestObjectiveState.Completed)
            {
                foreach(string str in stage.triggerFlagList)
                {
                    VLY_FlagManager.TriggerFlag(str);
                }

                foreach (string str in stage.incrementFlagList)
                {
                    VLY_FlagManager.IncrementFlagValue(str,1);
                }
            }
        }
    }

    /// <summary>
    /// Cherche après le behavior correspondant à l'objectif voulut.
    /// </summary>
    /// <param name="objective">L'objectif voulut.</param>
    /// <returns>Renvoi le behavior correspondant.</returns>
    private QST_ObjectiveBehavior GetObjectiveBehavior(QST_Objective objective)
    {
        for(int i = 0; i < objectivesBehaviors.Count; i++)
        {
            if(objectivesBehaviors[i].GetObjectiveType() == objective.GetType())
            {
                return objectivesBehaviors[i];
            }
        }

        return null;
    }

    private QST_RewardBehavior GetRewardBehavior(QST_Reward reward)
    {
        for (int i = 0; i < rewardBehaviors.Count; i++)
        {
            if (rewardBehaviors[i].GetRewardType() == reward.GetType())
            {
                return rewardBehaviors[i];
            }
        }

        return null;
    }

    //Ajouter un système de Key pour les evénements spéciaux (Répration de pont, ...)
}
