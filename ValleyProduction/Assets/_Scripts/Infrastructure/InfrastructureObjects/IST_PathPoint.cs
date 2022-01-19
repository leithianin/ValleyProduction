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
        if(PathManager.CanDeleteGameobject(this))
        {
            return true;
        }

        return false;
    }

    protected override void OnMoveObject()
    {
        Debug.Log("Moving PathPoint");
    }

    //Pas sur de ce que je fais là
    protected override void OnHoldRightClic()
    {
        PathManager.CreatePathData();
        PathManager.PlacePoint(this, transform.position);
    }

    protected override void OnSelectObject()
    {
        if(!PathManager.IsPathpointListEmpty())
        {
            PathManager.PlacePoint(this, transform.position);
        }
        else
        {
            //Check si plusieurs PathData
            if (PathManager.HasManyPath(this))
            {
                UIManager.ArrangePathButton(this);
            }
            else
            {
                PathManager.SelectPath(this);
            }
        }
    }

    protected override void OnUnselectObject()
    {
        Debug.Log("Unselect Pathpoint");
    }
}
