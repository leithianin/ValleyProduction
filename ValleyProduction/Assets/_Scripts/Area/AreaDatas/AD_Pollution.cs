using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AD_Pollution : AreaData<PollutionTrash>
{
    private List<PollutionTrash> trashesInZones = new List<PollutionTrash>();

    public override AreaDataType GetDataType()
    {
        return AreaDataType.Pollution;
    }

    protected override void OnAddData(PollutionTrash data)
    {
        if (!trashesInZones.Contains(data))
        {
            trashesInZones.Add(data);
        }
    }

    protected override void OnRemoveData(PollutionTrash data)
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
            toReturn += trashesInZones[i].pollutionLevel;
        }

        return Mathf.RoundToInt(toReturn);
    }
}
