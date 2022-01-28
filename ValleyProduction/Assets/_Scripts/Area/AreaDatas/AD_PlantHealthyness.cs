using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AD_PlantHealthyness : AreaData<TreeBehavior>
{
    private List<TreeBehavior> treesInArea = new List<TreeBehavior>();

    public override AreaDataType GetDataType()
    {
        return AreaDataType.Flora;
    }

    protected override void OnAddData(TreeBehavior data)
    {
        treesInArea.Add(data);
    }

    protected override void OnRemoveData(TreeBehavior data)
    {
        treesInArea.Remove(data);
    }

    protected override int ScoreCalculation()
    {
        int toReturn = 0;

        for(int i = 0; i < treesInArea.Count; i++)
        {
            toReturn += treesInArea[i].currentScore;
        }

        return toReturn;
    }
}
