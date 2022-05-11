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
        validationButton.interactable = false;

        handler.SetActive(false);
    }

    public void SetPendingCompletion()
    {
        validationButton.interactable = true;
    }

    public void SetQuestStage(VLY_Quest quest, List<QST_Objective> objectives)
    {
        if(!handler.activeSelf)
        {
            handler.SetActive(true);
        }

        if(quest != displayedQuest)
        {
            displayedQuest = quest;
            title.text = quest.questName;
        }

        //Voir comment on affiche les objectifs complétés
        string objectiveText = "";

        for(int i = 0; i < objectives.Count; i++)
        {
            objectiveText += objectives[i].Description + " \n";
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
