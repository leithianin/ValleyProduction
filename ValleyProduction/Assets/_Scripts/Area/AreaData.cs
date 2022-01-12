using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://stackoverflow.com/questions/353126/c-sharp-multiple-generic-types-in-one-list
public abstract class AreaData
{

}

public abstract class AreaData<T> : AreaData
{
    protected int score = 0;

    public int GetScore => score;

    public Type GetScoreType => typeof(T);

    public abstract void AddData(T data);

    public abstract void RemoveData(T data);

    public abstract int CalculateScore();
}
