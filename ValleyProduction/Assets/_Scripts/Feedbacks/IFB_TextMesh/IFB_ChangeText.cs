using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IFB_ChangeText : MonoBehaviour, IFeedbackPlayer
{
    public TMP_Text text;
    private string stringText = string.Empty;

    public TextsDictionary textsList;
    private TextBase txt;

    public void Play()
    {
        text.text = $"{txt.Title}\n{txt.Description}";
    }

    public void UpdateGoal(string id)
    {
        txt = textsList.GetTextAsset(id);
        Play();
    }
}
