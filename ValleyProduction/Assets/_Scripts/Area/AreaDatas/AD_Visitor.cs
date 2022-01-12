using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AD_Visitor : AreaData<VisitorBehavior>
{
    public override void AddData(VisitorBehavior data)
    {
        Debug.Log("Visitor Data");
    }

    public override int CalculateScore()
    {
        throw new System.NotImplementedException();
    }

    public override void RemoveData(VisitorBehavior data)
    {
        throw new System.NotImplementedException();
    }
}
