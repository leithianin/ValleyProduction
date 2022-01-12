using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://stackoverflow.com/questions/353126/c-sharp-multiple-generic-types-in-one-list
public abstract class AreaData
{
    /// Score actuel de l'Area.
    protected int score = 0;
    /// Getter du Score
    public int GetScore => score;
    /// Callback appel� � chaque fois que le score est modif�.
    public Action<int> OnUpdateScore;

    /// <summary>
    /// Renvoit le type de data pris en compte par le script.
    /// </summary>
    /// <returns>Type de data g�r� par le script.</returns>
    public abstract AreaDataType GetDataType();

    /// <summary>
    /// Calcule le score.
    /// </summary>
    /// <returns>Score calcul�.</returns>
    public abstract int CalculateScore();
}

public abstract class AreaData<T> : AreaData
{
    /// <summary>
    /// Appel� quand on ajoute une nouvelle data.
    /// </summary>
    /// <param name="data">La data ajout�e.</param>
    protected abstract void OnAddData(T data);

    /// <summary>
    /// Appel� quand on retire de la data.
    /// </summary>
    /// <param name="data">La data � retirer.</param>
    protected abstract void OnRemoveData(T data);

    /// <summary>
    /// Appel� quand on calcul le score. Permet de faire des calculs diff�rents pour chaque type de gestionnaire.
    /// </summary>
    /// <returns>Score calcul�.</returns>
    protected abstract int ScoreCalculation();

    /// <summary>
    /// Ajoute de la data
    /// </summary>
    /// <param name="data">La data � ajouter.</param>
    public void AddData(T data)
    {
        OnAddData(data);
        CalculateScore();
    }

    /// <summary>
    /// Retire de la data.
    /// </summary>
    /// <param name="data">La data � retirer.</param>
    public void RemoveData(T data)
    {
        OnRemoveData(data);
        CalculateScore();
    }

    /// <summary>
    /// Calcul le score.
    /// </summary>
    /// <returns>Score calcul�.</returns>
    public override int CalculateScore()
    {
        score = ScoreCalculation();
        OnUpdateScore?.Invoke(score);

        return score;
    }
}
