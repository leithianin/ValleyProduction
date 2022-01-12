using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://stackoverflow.com/questions/353126/c-sharp-multiple-generic-types-in-one-list
public abstract class AreaData
{
    protected int score = 0;

    public int GetScore => score;

    public Action<int> OnUpdateScore;

    public abstract AreaDataType GetDataType();

    public abstract int CalculateScore();
}

public abstract class AreaData<T> : AreaData
{
    protected abstract void OnAddData(T data);

    protected abstract void OnRemoveData(T data);

    protected abstract int ScoreCalculation();

    public void AddData(T data)
    {
        OnAddData(data);
        CalculateScore();
    }

    public void RemoveData(T data)
    {
        OnRemoveData(data);
        CalculateScore();
    }

    public override int CalculateScore()
    {
        score = ScoreCalculation();
        OnUpdateScore?.Invoke(score);

        return score;
    }
}
