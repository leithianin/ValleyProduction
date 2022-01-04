using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ISTP_PathPoint : InfrastructurePreview
{
    protected override void OnAskToPlace(Vector3 position)
    {
        Debug.Log("Ask Pathpoint preview");
    }

    protected override bool OnCanPlaceObject(Vector3 position)
    {
        Debug.Log("Can Place Pathpoint preview");
        return true;
    }
}
