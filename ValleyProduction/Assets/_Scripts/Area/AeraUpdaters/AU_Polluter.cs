using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AU_Polluter : AreaUpdater<PollutionTrash>
{
    public override void SetData(PollutionTrash newData)
    {
        data = newData;
    }
}
