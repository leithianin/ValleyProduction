using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AU_Polluter : EcosystemAgent<AU_Polluter>
{
    public override void SetData(AU_Polluter newData)
    {
        data = newData;
    }

    public override void SetData()
    {
        data = this;
    }
}
