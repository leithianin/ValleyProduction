using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IFB_ChangeTextGoal : MonoBehaviour, IFeedbackPlayer
{
    public RectTransform rtGoalDescription;
    public RectTransform rtGoalBar;

    public TMP_Text description;
    public TMP_Text title;
    private string stringText = string.Empty;

    public TextsDictionary textsList;
    private TextBase txt;

    public void Play()
    {
        if (txt.Title != string.Empty) { title.text = $"{txt.Title}"; }
        description.text = $"{txt.Description}";

        UpdateBackgroundSize();
    }

    public void UpdateGoal(string id)
    {
        txt = textsList.GetTextAsset(id);
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
