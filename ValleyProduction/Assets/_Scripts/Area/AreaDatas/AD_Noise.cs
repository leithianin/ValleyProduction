using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AD_Noise : AreaData<VisitorBehavior>
{
    List<VisitorBehavior> visitorInZone = new List<VisitorBehavior>();

    public override AreaDataType GetDataType()
    {
        return AreaDataType.Noise;
    }

    protected override void OnAddData(VisitorBehavior data)
    {
        visitorInZone.Add(data);
    }

    protected override void OnRemoveData(VisitorBehavior data)
    {
        visitorInZone.Remove(data);
    }

    protected override int ScoreCalculation()
    {
        return visitorInZone.Count;
    }
}
