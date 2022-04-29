using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ECO_AGT_Polluter : EcosystemAgent<ECO_AGT_Polluter>
{
    public override void SetData(ECO_AGT_Polluter newData)
    {
        data = newData;
    }

    public override void SetData()
    {
        data = this;
    }
    public override EcosystemDataType UsedDataType()
    {
        return EcosystemDataType.Pollution;
    }
}
