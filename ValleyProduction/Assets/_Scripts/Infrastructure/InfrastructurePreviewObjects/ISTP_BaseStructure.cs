using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ISTP_BaseStructure : InfrastructurePreview
{
    protected override void OnAskToPlace(Vector3 position)
    {
        
    }

    protected override bool OnCanPlaceObject(Vector3 position)
    {
        return true;
    }
}
