using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IFB_ChangeText : MonoBehaviour, IFeedbackPlayer
{
    public TMP_Text description;
    public TMP_Text title;
    private string stringText = string.Empty;

    public TextsDictionary textsList;
    private TextBase txt;

    public void Play()
    {
        description.text = $"{txt.Description}";
        title.text = $"{txt.Title}";

    }

    public void UpdateGoal(string id)
    {
        txt = textsList.GetTextAsset(id);
        Play();
    }

    public void UpdateBackgroundSize()
    {
        //Carré noir ref = preferedSize du texte //         var bgSize = new Vector2(300, tooltip.preferredHeight + 20);
        //Barre changer height *1.03 
    }
}
