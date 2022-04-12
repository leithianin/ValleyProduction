using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VLY_QuestManager : VLY_Singleton<VLY_QuestManager>
{
    [SerializeField] private VLY_Quest[] allQuests;

    //Quest objectives behavior
    private List<QST_ObjectiveBehavior> objectivesBehaviors = new List<QST_ObjectiveBehavior>();

    //Quest rewards behavior
    

    private void Start()
    {
        //Cr�ation des diff�rents behavior pour les Objectifs
        objectivesBehaviors.Add(new QST_OBJB_Ressource());

        //R�cup�ration des qu�tes dans le projet.
        allQuests = Resources.FindObjectsOfTypeAll<VLY_Quest>();

        //R�initialisaiton des qu�tes
        for(int i = 0; i < allQuests.Length; i++)
        {
            allQuests[i].Reset();
            for(int j = 0; j < allQuests[i].Objectives.Count; j++)
            {
                allQuests[i].Objectives[j].Reset();
            }
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
    /// V�rifie l'�tat de la qu�te et met � jour le prochain objectif.
    /// </summary>
    /// <param name="updatedQuest">La qu�te � update.</param>
    private void UpdateQuestObjective(VLY_Quest updatedQuest)
    {
        int i = 0;

        for (i = 0; i < updatedQuest.Objectives.Count; i++)
        {
            if(updatedQuest.Objectives[i].State == QuestObjectiveState.Completed) //On ignore objectifs d�j� remplis
            {
                continue;
            }
            else if(updatedQuest.Objectives[i].State == QuestObjectiveState.Pending) //Si un objectif n'est pas commenc�, on le commence.
            {
                SetObjectiveStatus(updatedQuest.Objectives[i], QuestObjectiveState.Started);

                updatedQuest.Objectives[i].OnUpdateState += (QuestObjectiveState state) => instance.UpdateQuestObjective(updatedQuest);
                break; //On sort de la boucle d�s qu'un nouvel objectif est lanc�.
            }
        }

        if(i >= updatedQuest.Objectives.Count)
        {
            updatedQuest.state = QuestObjectiveState.Completed; //CODE REVIEW : Voir pour le mettre dans une fonction (G�rer les feedbacks)
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

    //Ajouter un syst�me de Key pour les ev�nements sp�ciaux (R�pration de pont, ...)
}
