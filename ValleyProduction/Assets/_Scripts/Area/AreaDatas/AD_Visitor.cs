using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AD_Visitor : AreaData<int>
{
    public override AreaDataType GetDataType()
    {
        return AreaDataType.Noise;
    }

    protected override void OnAddData(int data)
    {
        Debug.Log("Visitor Data");
    }

    protected override int ScoreCalculation()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnRemoveData(int data)
    {
        throw new System.NotImplementedException();
    }
}
