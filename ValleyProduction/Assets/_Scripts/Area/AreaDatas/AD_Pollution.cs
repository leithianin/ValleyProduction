using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AD_Pollution : AreaData<AU_Polluter>
{
    private List<AU_Polluter> trashesInZones = new List<AU_Polluter>();

    public override AreaDataType GetDataType()
    {
        return AreaDataType.Pollution;
    }

    protected override void OnAddData(AU_Polluter data)
    {
        if (!trashesInZones.Contains(data))
        {
            trashesInZones.Add(data);
        }
    }

    protected override void OnRemoveData(AU_Polluter data)
    {
        if (trashesInZones.Contains(data))
        {
            trashesInZones.Remove(data);
        }
    }

    protected override int ScoreCalculation()
    {
        float toReturn = 0;
        for(int i = 0; i < trashesInZones.Count; i++)
        {
            toReturn += trashesInZones[i].GetScore;
        }

        return Mathf.RoundToInt(toReturn);
    }
}
