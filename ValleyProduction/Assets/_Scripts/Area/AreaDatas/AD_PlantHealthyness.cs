using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AD_PlantHealthyness : AreaData<AU_PlantCounter>
{
    private List<AU_PlantCounter> treesInArea = new List<AU_PlantCounter>();

    public override AreaDataType GetDataType()
    {
        return AreaDataType.Flora;
    }

    protected override void OnAddData(AU_PlantCounter data)
    {
        treesInArea.Add(data);
    }

    protected override void OnRemoveData(AU_PlantCounter data)
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
