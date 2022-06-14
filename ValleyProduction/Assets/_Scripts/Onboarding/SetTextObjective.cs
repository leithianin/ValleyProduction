using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetTextObjective : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public TextObjective objective;

    private void OnEnable()
    {
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
