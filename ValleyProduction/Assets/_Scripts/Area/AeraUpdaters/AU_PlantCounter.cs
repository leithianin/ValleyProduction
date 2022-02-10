using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AU_PlantCounter : AreaUpdater<AU_PlantCounter>
{
    public override void SetData(AU_PlantCounter newData)
    {
        data = newData;
    }

    public override void SetData()
    {
        data = this;
    }
}
