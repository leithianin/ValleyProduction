using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://stackoverflow.com/questions/353126/c-sharp-multiple-generic-types-in-one-list
[Serializable, System.Obsolete]
public abstract class AreaData
{
    public bool needUpdate = false;

    public Area linkedArea;
    /// Score actuel de l'Area.
    /// 
    private int currentScore;

    public int Score => currentScore;

    protected int realScore = 0;
    /// Getter du Score
    public int GetRealScore => realScore;
    /// Callback appelé à chaque fois que le score est modifé.
    public Action<int> OnUpdateScore;

    /// <summary>
    /// Renvoit le type de data pris en compte par le script.
    /// </summary>
    /// <returns>Type de data géré par le script.</returns>
    public abstract EcosystemDataType GetDataType();

    /// <summary>
    /// Calcule le score.
    /// </summary>
    /// <returns>Score calculé.</returns>
    public abstract int CalculateScore();

    /// <summary>
    /// Appelé quand l'un des voisins a mit à jour son score.
    /// </summary>
    public abstract void UpdateScoreFromNeighbours();

    protected void SetScore(int nScore)
    {
        currentScore = nScore;

        needUpdate = true;
    }

    public void UpdateScoreFeedbacks()
    {
        OnUpdateScore?.Invoke(currentScore);

        needUpdate = false;
    }
}

/// <summary>
/// Gère le calcul du score à partir des objets de type T contenant la data (Exemple : voir le AD_Noise (système de bruit))
/// </summary>
/// <typeparam name="T">Type d'objet à prendre en compte.</typeparam>
[Serializable,Obsolete]
public abstract class AreaData<T> : AreaData
{
    /// <summary>
    /// Appelé quand on ajoute une nouvelle data.
    /// </summary>
    /// <param name="data">La data ajoutée.</param>
    protected abstract void OnAddData(T data);

    /// <summary>
    /// Appelé quand on retire de la data.
    /// </summary>
    /// <param name="data">La data à retirer.</param>
    protected abstract void OnRemoveData(T data);

    /// <summary>
    /// Appelé quand on calcul le score. Permet de faire des calculs différents pour chaque type de gestionnaire.
    /// </summary>
    /// <returns>Score calculé.</returns>
    protected abstract int ScoreCalculation();

    /// <summary>
    /// Ajoute de la data
    /// </summary>
    /// <param name="data">La data à ajouter.</param>
    public void AddData(T data)
    {
        OnAddData(data);
        CalculateScore();
    }

    /// <summary>
    /// Retire de la data.
    /// </summary>
    /// <param name="data">La data à retirer.</param>
    public void RemoveData(T data)
    {
        OnRemoveData(data);
        CalculateScore();
    }

    /// <summary>
    /// Update la data.
    /// </summary>
    /// <param name="dataToRemove">L'ancienne valeur de la data.</param>
    /// <param name="dataToAdd">La nouvelle valeur de la data.</param>
    public void RefreshData(T dataToRemove, T dataToAdd)
    {
        OnRemoveData(dataToRemove);
        OnAddData(dataToAdd);
        CalculateScore();
    }

    /// <summary>
    /// Calcul le score.
    /// </summary>
    /// <returns>Score calculé.</returns>
    public override int CalculateScore()
    {
        realScore = ScoreCalculation();

        List<Area> neighbours = AreaManager.GetNeighbours(linkedArea);

        int neighboursScore = 0;

        for(int i = 0; i < neighbours.Count; i++)
        {
            List<AreaData<T>> neighboursDatas = neighbours[i].GetData<T>();

            for(int j = 0; j < neighboursDatas.Count; j++)
            {
                neighboursScore += Mathf.RoundToInt(neighboursDatas[j].GetRealScore * 0.5f);
                neighboursDatas[j].UpdateScoreFromNeighbours();
            }
        }

        SetScore(realScore + neighboursScore);

        return realScore;
    }

    /// <summary>
    /// Calcul le score sans calculer celui des voisins.
    /// </summary>
    public override void UpdateScoreFromNeighbours()
    {
        List<Area> neighbours = AreaManager.GetNeighbours(linkedArea);

        int neighboursScore = 0;

        for (int i = 0; i < neighbours.Count; i++)
        {
            List<AreaData<T>> neighboursDatas = neighbours[i].GetData<T>();

            for (int j = 0; j < neighboursDatas.Count; j++)
            {
                neighboursScore += Mathf.RoundToInt(neighboursDatas[j].GetRealScore * 0.5f);
            }
        }

        SetScore(realScore + neighboursScore);
    }
}
