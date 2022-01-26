using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AreaDisplayDataHandler
{
    public AreaDataType dataTypeToCheck;
    [SerializeField] private int wantedScore;
    [SerializeField] private bool needHigher = true;
    [HideInInspector] public int score;

    public bool IsValid => (score >= wantedScore && needHigher) || (score <= wantedScore && !needHigher);
}

public abstract class AreaDisplay : MonoBehaviour
{
    /// Liste des type de data utilisé par l'AreaDisplay et leur degré d'importance.
    [SerializeField] private List<AreaDisplayDataHandler> scoreData;

    public Vector2 Position => new Vector2(transform.position.x, transform.position.z);


    /// <summary>
    /// Appelé quand le score est modifié.
    /// </summary>
    /// <param name="newScore">Le nouveau score.</param>
    public abstract void OnUpdateScore(int newScore);

    /// <summary>
    /// S'inscrit à l'action "OnUpdateScore" des Area voulues.
    /// </summary>
    /// <param name="possibleDatas">Liste des gestionaire de data continue dans l'Area.</param>
    public void AffectToArea(List<AreaData> possibleDatas)
    {
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
        int reachedDataLevels = 0;
        for(int i = 0; i < scoreData.Count; i++)
        {
            if(scoreData[i].dataTypeToCheck == data)
            {
                scoreData[i].score = score;
            }

            if(scoreData[i].IsValid)
            {
                reachedDataLevels++;
            }
        }

        OnUpdateScore(reachedDataLevels);
    }

    /// <summary>
    /// Vérifie si l'AreaDisplay a besoin d'écouter un type de data.
    /// </summary>
    /// <param name="dataType">Le type de data voulu.</param>
    /// <returns></returns>
    private bool NeedDataType(AreaDataType dataType)
    {
        bool toReturn = false;

        for (int i = 0; i < scoreData.Count; i++)
        {
            if(scoreData[i].dataTypeToCheck == dataType)
            {
                toReturn = true;
            }
        }

        return toReturn;
    }
}
