using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete]
public class AD_PlantHealthyness : AreaData<ECO_AGT_PlantCounter>
{
    private List<ECO_AGT_PlantCounter> treesInArea = new List<ECO_AGT_PlantCounter>();

    public override EcosystemDataType GetDataType()
    {
        return EcosystemDataType.Flora;
    }

    protected override void OnAddData(ECO_AGT_PlantCounter data)
    {
        treesInArea.Add(data);
    }

    protected override void OnRemoveData(ECO_AGT_PlantCounter data)
    {
        treesInArea.Remove(data);
    }

    protected override int ScoreCalculation()
    {
        float toReturn = 0;

        for(int i = 0; i < treesInArea.Count; i++)
        {
            toReturn += treesInArea[i].GetScore();
        }

        return Mathf.RoundToInt(toReturn);
    }
}
