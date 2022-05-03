using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EcosystemScoreHandler
{
    public List<EcosystemDisplayDataHandler> displayDatas;

    public bool IsValid()
    {
        bool toReturn = true;

        for(int i = 0; i < displayDatas.Count; i++)
        {
            if(!displayDatas[i].IsValid)
            {
                toReturn = false;
                break;
            }
        }

        return toReturn;
    }
}

[Serializable]
public class EcosystemDisplayDataHandler
{
    public EcosystemDataType dataTypeToCheck;
    [SerializeField] private int wantedScore;
    [SerializeField] private bool needHigher = true;
     public int score;

    public bool IsValid => (score >= wantedScore && needHigher) || (score <= wantedScore && !needHigher);
}

public abstract class EcosystemDisplay : MonoBehaviour
{
    /// Liste des type de data utilisé par l'AreaDisplay et leur degré d'importance.
    [SerializeField] private List<EcosystemScoreHandler> scoreData;

    public Vector2 Position => new Vector2(transform.position.x, transform.position.z);


    private void Start()
    {
        OnStart();
    }

    protected virtual void OnStart()
    {

    }

    private void Update()
    {
        for (int i = 0; i < scoreData.Count; i++)
        {
            for (int j = 0; j < scoreData[i].displayDatas.Count; j++)
            {
                UpdateData(VLY_EcosystemManager.GetScoreAtPosition(Position, scoreData[i].displayDatas[j].dataTypeToCheck), scoreData[i].displayDatas[j].dataTypeToCheck);
            }
        }
    }

    /// <summary>
    /// Appelé quand le score est modifié.
    /// </summary>
    /// <param name="newScore">Le nouveau score.</param>
    public abstract void OnUpdateScore(int newScore);

    /// <summary>
    /// Calcul le nouveau score total et Update la data en fonction.
    /// </summary>
    /// <param name="score">Le nouveau score.</param>
    /// <param name="data">Le type de data voulu.</param>
    private void UpdateData(int score, EcosystemDataType data)
    {
        int reachedDataLevels = 0;
        for(int i = 0; i < scoreData.Count; i++)
        {
            for (int j = 0; j < scoreData[i].displayDatas.Count; j++)
            {
                if (scoreData[i].displayDatas[j].dataTypeToCheck == data)
                {
                    scoreData[i].displayDatas[j].score = score;
                }
            }

            if (scoreData[i].IsValid())
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
    private bool NeedDataType(EcosystemDataType dataType)
    {
        bool toReturn = false;

        for (int i = 0; i < scoreData.Count; i++)
        {
            for (int j = 0; j < scoreData[i].displayDatas.Count; j++)
            {
                if (scoreData[i].displayDatas[j].dataTypeToCheck == dataType)
                {
                    toReturn = true;
                }
            }
        }

        return toReturn;
    }
}
