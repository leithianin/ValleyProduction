using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IST_PathPoint : Infrastructure
{
    protected override void OnPlaceObject(Vector3 position)
    {
        PathManager.PlacePoint(this, position);      
    }

    protected override void OnRemoveObject()
    {
        Debug.Log("Remove Pathpoint");
    }

    protected override void OnSelectObject()
    {
        Debug.Log("Select Pathpoint");
    }

    protected override void OnUnselectObject()
    {
        Debug.Log("Unselect Pathpoint");
    }
}
