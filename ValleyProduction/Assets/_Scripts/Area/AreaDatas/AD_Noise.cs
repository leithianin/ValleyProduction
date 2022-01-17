using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AD_Noise : AreaData<int>
{
    int reScore;

    public override AreaDataType GetDataType()
    {
        return AreaDataType.Noise;
    }

    protected override void OnAddData(int data)
    {
        reScore += data;
    }

    protected override void OnRemoveData(int data)
    {
        reScore -= data;
    }

    protected override int ScoreCalculation()
    {
        return reScore;
    }
}
