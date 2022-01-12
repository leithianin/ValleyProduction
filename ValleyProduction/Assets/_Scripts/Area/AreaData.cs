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

    public abstract int CalculateScore();
}

public abstract class AreaData<T> : AreaData
{
    public abstract void AddData(T data);

    public abstract void RemoveData(T data);

    protected abstract int ScoreCalculation();

    public override int CalculateScore()
    {
        score = ScoreCalculation();
        OnUpdateScore(score);

        return score;
    }
}
