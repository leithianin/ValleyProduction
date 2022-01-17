using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AreaDisplayDataHandler
{
    public AreaDataType dataTypeToCheck;
    [HideInInspector] public int score;
    public float coef;
}

public abstract class AreaDisplay : MonoBehaviour
{
    /// Liste des type de data utilisé par l'AreaDisplay et leur degré d'importance.
    [SerializeField] private List<AreaDisplayDataHandler> datas;

    /// <summary>
    /// Appelé quand le score est modifié.
    /// </summary>
    /// <param name="newScore">Le nouveau score.</param>
    public abstract void OnUpdateScore(float newScore);

    /// <summary>
    /// S'inscrit à l'action "OnUpdateScore" des Area voulues.
    /// </summary>
    /// <param name="possibleDatas">Liste des gestionaire de data continue dans l'Area.</param>
    public void AffectToArea(List<AreaData> possibleDatas)
    {
        // CODE REVIEW : Voir comment on différencie quel display est affecté par quel score
        for (int i = 0; i < possibleDatas.Count; i++)
        {
            AreaData areaData = possibleDatas[i];

            if (NeedDataType(areaData.GetDataType()))
            {
                areaData.OnUpdateScore += (int s) => UpdateData(s, areaData.GetDataType());
            }
        }
    }

    /// <summary>
    /// Calcul le nouveau score total et Update la data en fonction.
    /// </summary>
    /// <param name="score">Le nouveau score.</param>
    /// <param name="data">Le type de data voulu.</param>
    private void UpdateData(int score, AreaDataType data)
    {
        int totalScore = 0;
        float totalCoef = 0;
        for(int i = 0; i < datas.Count; i++)
        {
            if(datas[i].dataTypeToCheck == data)
            {
                datas[i].score = score;
            }

            totalScore += datas[i].score;
            totalCoef += datas[i].coef;
        }

        OnUpdateScore(totalScore / totalCoef);
    }

    /// <summary>
    /// Vérifie si l'AreaDisplay a besoin d'écouter un type de data.
    /// </summary>
    /// <param name="dataType">Le type de data voulu.</param>
    /// <returns></returns>
    private bool NeedDataType(AreaDataType dataType)
    {
        bool toReturn = false;

        for (int i = 0; i < datas.Count; i++)
        {
            if(datas[i].dataTypeToCheck == dataType)
            {
                toReturn = true;
            }
        }

        return toReturn;
    }
}
