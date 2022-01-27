using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AU_PlantCounter : AreaUpdater<TreeBehavior>
{
    public override void SetData(TreeBehavior newData)
    {
        data = newData;
    }
}
