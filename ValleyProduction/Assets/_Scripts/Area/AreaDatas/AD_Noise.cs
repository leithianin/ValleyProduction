using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AD_Noise : AreaData<CPN_Movement>
{
    public override void AddData(CPN_Movement data)
    {
        Debug.Log("Noise Data");
    }

    public override void RemoveData(CPN_Movement data)
    {
        throw new System.NotImplementedException();
    }

    protected override int ScoreCalculation()
    {
        throw new System.NotImplementedException();
    }
}
