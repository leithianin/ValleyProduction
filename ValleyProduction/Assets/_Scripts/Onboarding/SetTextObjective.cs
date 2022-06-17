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
                break;

            case Language.fr:
                if(objective.Textfr != string.Empty) { textMesh.text = objective.Textfr; }
                else { textMesh.text = objective.Texten; }
                break;
        }

        title.text = objective.Title;
    }

    private void UpdateText()
    {
        switch (UIManager.GetData.lang)
        {
            case Language.en:
                textMesh.text = objective.Texten;
                break;

            case Language.fr:
                if (objective.Textfr != string.Empty) { textMesh.text = objective.Textfr; }
                else { textMesh.text = objective.Texten; }
                break;
        }
    }
}
