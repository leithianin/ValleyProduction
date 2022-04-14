using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_QuestDisplayer : MonoBehaviour
{
    [SerializeField] private List<IFB_ChangeTextGoal> questObjectives = new List<IFB_ChangeTextGoal>();

    public void SetQuestObjective(VLY_Quest quest, List<QST_Objective> objectives)
    {
        for(int i = 0; i < questObjectives.Count; i++)
        {
            if(questObjectives[i].DisplayedQuest == null || questObjectives[i].DisplayedQuest == quest)
            {
                questObjectives[i].SetQuestStage(quest, objectives);

                break;
            }
        }
    }

    public void SetPendingCompletion(VLY_Quest quest)
    {
        for (int i = 0; i < questObjectives.Count; i++)
        {
            if (questObjectives[i].DisplayedQuest == quest)
            {
                questObjectives[i].SetPendingCompletion();

                break;
            }
        }
    }

    public void ValidateQuest(int id)
    {
        VLY_QuestManager.CompleteQuest(questObjectives[id].DisplayedQuest);
        questObjectives[id].ResetDisplay();
    }
}
