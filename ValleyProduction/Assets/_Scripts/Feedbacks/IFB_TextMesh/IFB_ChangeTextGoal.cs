using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IFB_ChangeTextGoal : MonoBehaviour, IFeedbackPlayer // CODE REVIEW : A renommer (C'est pas un feedback)
{
    public RectTransform rtGoalDescription;
    public RectTransform rtGoalBar;

    public TMP_Text description;
    public TMP_Text title;
    [SerializeField] private Button validationButton;
    [SerializeField] private GameObject handler;
    private string stringText = string.Empty;

    public TextsDictionary textsList;
    private TextBase txt;

    private VLY_Quest displayedQuest;

    public VLY_Quest DisplayedQuest => displayedQuest;

    private FontStyles newFont;

    private VLY_Quest savedQuest;
    private List<QST_Objective> savedObjectives;

    public void Play()
    {
        if (txt.Title != string.Empty) { title.text = $"{txt.Title}"; }
        description.text = $"{txt.Texts}";

        UpdateBackgroundSize();
    }

    public void ResetDisplay()
    {
        displayedQuest = null;
        title.text = "";
        description.text = "";
        validationButton.gameObject.SetActive(false);

        handler.SetActive(false);
    }

    public void SetPendingCompletion()
    {
        description.text = "<s>" + description.text;
        validationButton.gameObject.SetActive(true);
    }

    public void ObjectiveValidated()
    {

    }

    public void UpdateText()
    {
        SetQuestStage(savedQuest, savedObjectives);

        switch (UIManager.GetData.lang)
        {
            case Language.en:
                title.text = savedQuest.questName;
                break;

            case Language.fr:
                if (savedQuest.questNamefr != string.Empty)
                {
                    title.text = savedQuest.questNamefr;
                }
                else
                {
                    title.text = savedQuest.questName;
                }
                break;
        }
    }

    public void SetQuestStage(VLY_Quest quest, List<QST_Objective> objectives)
    {
        UIManager.OnLanguageChange += UpdateText;
        savedQuest = quest;
        savedObjectives = objectives;

        if (!handler.activeSelf)
        {
            handler.SetActive(true);
        }

        if(quest != displayedQuest)
        {
            displayedQuest = quest;

            switch (UIManager.GetData.lang)
            {
                case Language.en:
                    title.text = quest.questName;
                    break;

                case Language.fr:
                    if (quest.questNamefr != string.Empty)
                    {
                        title.text = quest.questNamefr;
                    }
                    else
                    {
                        title.text = quest.questName;
                    }
                    break;
            }
        }

        //Voir comment on affiche les objectifs complétés
        string objectiveText = "";

        for(int i = 0; i < objectives.Count; i++)
        {
            if (objectives[i].State != QuestObjectiveState.Completed)
            {
                switch (UIManager.GetData.lang)
                {
                    case Language.en:
                        objectiveText += objectives[i].Description + " \n";
                        break;

                    case Language.fr:
                        if (objectives[i].Descriptionfr != string.Empty)
                        {
                            objectiveText += objectives[i].Descriptionfr + " \n";
                        }
                        else
                        {
                            objectiveText += objectives[i].Description + " \n";
                        }
                        break;
                }
            }
            else
            {
                switch (UIManager.GetData.lang)
                {
                    case Language.en:
                        objectiveText += "<s>" + objectives[i].Description + " </s> \n";
                        break;

                    case Language.fr:
                        if (objectives[i].Descriptionfr != string.Empty)
                        {
                            objectiveText += "<s>" + objectives[i].Descriptionfr + " </s> \n";
                        }
                        else
                        {
                            objectiveText += "<s>" + objectives[i].Description + " </s> \n";
                        }
                        break;
                }
            }
        }

        description.text = objectiveText;
        UpdateBackgroundSize();
    }

    public void UpdateGoal(string id)
    {
        txt = TextsDictionary.instance.GetTextAsset(id);
        Play();
    }

    public void UpdateBackgroundSize()
    {
        var bgSize = new Vector2(304, description.preferredHeight + 100);
        rtGoalDescription.sizeDelta = bgSize;

        bgSize = new Vector2(41, (description.preferredHeight + 250));
        rtGoalBar.sizeDelta = bgSize;
    }
}
