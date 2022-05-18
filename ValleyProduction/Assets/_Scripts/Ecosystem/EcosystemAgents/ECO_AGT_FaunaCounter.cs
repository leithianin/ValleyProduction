using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ECO_AGT_FaunaCounter : EcosystemAgent<ECO_AGT_FaunaCounter>
{
    public override void SetData(ECO_AGT_FaunaCounter newData)
    {
        throw new System.NotImplementedException();
    }

    public override void SetData()
    {
        data = this;
    }

    public override EcosystemDataType UsedDataType()
    {
        return EcosystemDataType.Fauna;
    }
}
