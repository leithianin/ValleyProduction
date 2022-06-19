using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetTextObjective : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public TextMeshProUGUI title;
    public TextObjective objective;

    private void OnEnable()
    {
        Debug.Log("OnEnable");

        UIManager.OnLanguageChange += UpdateText;
        //Need to me désinscrire aussi à un moment

        switch (UIManager.GetData.lang)
        {
            case Language.en:
                textMesh.text = objective.Texten;
                title.text = objective.Title;
                break;

            case Language.fr:
                if(objective.Textfr != string.Empty) 
                { 
                    textMesh.text = objective.Textfr;
                    title.text = objective.Titlefr;
                }
                else 
                { 
                    textMesh.text = objective.Texten;
                    title.text = objective.Title;
                }
                break;
        }
    }

    private void UpdateText()
    {
        switch (UIManager.GetData.lang)
        {
            case Language.en:
                textMesh.text = objective.Texten;
                title.text = objective.Title;
                break;

            case Language.fr:
                if (objective.Textfr != string.Empty)
                {
                    textMesh.text = objective.Textfr;
                    title.text = objective.Titlefr;
                }
                else
                {
                    textMesh.text = objective.Texten;
                    title.text = objective.Title;
                }
                break;
        }
    }
}
