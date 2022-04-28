using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete]
public class AD_Pollution : AreaData<ECO_AGT_Polluter>
{
    private List<ECO_AGT_Polluter> trashesInZones = new List<ECO_AGT_Polluter>();

    public override EcosystemDataType GetDataType()
    {
        return EcosystemDataType.Pollution;
    }

    protected override void OnAddData(ECO_AGT_Polluter data)
    {
        if (!trashesInZones.Contains(data))
        {
            trashesInZones.Add(data);
        }
    }

    protected override void OnRemoveData(ECO_AGT_Polluter data)
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
            toReturn += trashesInZones[i].GetScore();
        }

        return Mathf.RoundToInt(toReturn);
    }
}
