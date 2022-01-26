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
        /*if (PathManager.HasManyPath(this))
        {
            UIManager.ArrangePathButton(this);
        }*/

        /*if (!PathManager.IsOnCurrentPathData(this))
        {
            PathManager.CreatePathData();
            PathManager.SelectPath(this);
        }*/

        if (PathManager.CanDeleteGameobject(this))
        {
            return true;
        }

        return false;
    }

    protected override void OnMoveObject()
    {
        Debug.Log("Moving PathPoint");
    }

    protected override void OnReplaceObject()
    {
        PathManager.UpdateLineWhenMoving(this);
    }

    //Pas sur de ce que je fais là
    protected override void OnHoldRightClic()
    {
        PathManager.CreatePathData();
        PathManager.PlacePoint(this, transform.position);
        UIManager.HideRoadsInfo();
    }

    protected override void OnSelectObject()
    {
        if (this == PathManager.previousPathpoint)
        {
            PathManager.CreatePathData();
            UIManager.HideRoadsInfo();
        }
        else
        {
            if (!PathManager.IsPathpointListEmpty())
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
    }

    protected override void OnUnselectObject()
    {
        Debug.Log("Unselect Pathpoint");
    }
}
