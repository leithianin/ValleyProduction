using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IST_PathPoint : Infrastructure
{
    protected override void OnPlaceObject(Vector3 position)
    {
        PathManager.PlacePoint(this, position);      
    }

    protected override bool OnRemoveObject()
    {
        if(PathManager.CanDeletePoint(this))
        {
            return true;
        }

        return false;
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
