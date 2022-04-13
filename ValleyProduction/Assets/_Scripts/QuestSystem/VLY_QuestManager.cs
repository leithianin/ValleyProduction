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

    private void Start()
    {
        //Cr�ation des diff�rents behavior pour les Objectifs
        objectivesBehaviors.Add(new QST_OBJB_Ressource());
        objectivesBehaviors.Add(new QST_OBJB_PlaceStructure());

        //Cr�ation des diff�rents behavior pour les Rewards
        rewardBehaviors.Add(new QST_RWDB_Ressource());
        rewardBehaviors.Add(new QST_RWDB_UnlockStructure());
        rewardBehaviors.Add(new QST_RWDB_VisitorType());

        //R�cup�ration des qu�tes dans le projet.
        allQuests = Resources.FindObjectsOfTypeAll<VLY_Quest>();

        //R�initialisaiton des qu�tes
        for(int i = 0; i < allQuests.Length; i++)
        {
            allQuests[i].Reset();
        }

        //Placeholder : D�marre la premi�re qu�te
        foreach (VLY_Quest q in allQuests)
        {
            BeginQuest(q);
        }
    }

    /// <summary>
    /// Commence la qu�te si elle n'est pas d�j� avanc�e.
    /// </summary>
    /// <param name="quest">La qu�te � d�marrer</param>
    public static void BeginQuest(VLY_Quest quest)
    {
        if(quest.state == QuestObjectiveState.Pending)
        {
            instance.UpdateQuestObjective(quest);
        }
    }

    /// <summary>
    /// Appel� quand un qu�te se finit. Fait gagner les r�compenses au joueur.
    /// </summary>
    /// <param name="quest">La qu�te � finir.</param>
    private void CompleteQuest(VLY_Quest quest)
    {
        quest.state = QuestObjectiveState.Completed;

        for(int i = 0; i < quest.Rewards.Count; i++)
        {
            QST_RewardBehavior behavior = GetRewardBehavior(quest.Rewards[i]);

            behavior.GiveReward(quest.Rewards[i]);

            Debug.Log("Reward given : " + quest.Rewards[i]);
        }
    }

    /// <summary>
    /// V�rifie l'�tat de la qu�te et met � jour le prochain objectif.
    /// </summary>
    /// <param name="updatedQuest">La qu�te � update.</param>
    private void UpdateQuestObjective(VLY_Quest updatedQuest) //CODE REVIEW : Clean la fonction (S�parer la partie "Quest Stage" dans une autre fonction
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
                        updatedQuest.Stages[i].State = QuestObjectiveState.Completed;
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

                    updatedQuest.Stages[i].State = QuestObjectiveState.Started;

                    hasStartedObjective = true;
                    break;
            }

            if(hasStartedObjective)
            {
                break;
            }
        }

        if(i >= updatedQuest.Stages.Count)
        {
            CompleteQuest(updatedQuest);
        }
        else if(i <= 0)
        {
            updatedQuest.state = QuestObjectiveState.Started; //CODE REVIEW : Voir pour le mettre dans une fonction (G�rer les feedbacks)
        }
    }

    /// <summary>
    /// Met � jour l'�tat d'un objectif
    /// </summary>
    /// <param name="objective">L'objectif � mettre � jour.</param>
    /// <param name="state">L'�tat de l'objectif.</param>
    public static void SetObjectiveStatus(QST_Objective objective, QuestObjectiveState state)
    {
        QST_ObjectiveBehavior usedBehavior = instance.GetObjectiveBehavior(objective);

        usedBehavior.SetObjectiveStatus(objective, state);
    }

    /// <summary>
    /// Cherche apr�s le behavior correspondant � l'objectif voulut.
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

    //Ajouter un syst�me de Key pour les ev�nements sp�ciaux (R�pration de pont, ...)
}
