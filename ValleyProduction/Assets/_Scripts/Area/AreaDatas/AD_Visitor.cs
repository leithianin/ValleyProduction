using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AD_Visitor : AreaData<VisitorBehavior>
{
    public override AreaDataType GetDataType()
    {
        return AreaDataType.Noise;
    }

    protected override void OnAddData(VisitorBehavior data)
    {
        Debug.Log("Visitor Data");
    }

    protected override int ScoreCalculation()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnRemoveData(VisitorBehavior data)
    {
        throw new System.NotImplementedException();
    }
}
