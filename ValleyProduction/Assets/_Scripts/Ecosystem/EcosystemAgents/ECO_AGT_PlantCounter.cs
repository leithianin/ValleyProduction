using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ECO_AGT_PlantCounter : EcosystemAgent<ECO_AGT_PlantCounter>
{
    public override void SetData(ECO_AGT_PlantCounter newData)
    {
        data = newData;
    }

    public override void SetData()
    {
        data = this;
    }

    public override EcosystemDataType UsedDataType()
    {
        return EcosystemDataType.Flora;
    }
}
