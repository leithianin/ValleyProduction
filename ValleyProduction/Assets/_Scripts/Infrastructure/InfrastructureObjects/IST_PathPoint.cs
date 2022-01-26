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
        //PathManager.UpdateLineWhenMoving(this);
    }

    protected override void OnReplaceObject()
    {
        PathManager.UpdateAfterMoving(this);
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
                    if(PathManager.HasOnePath(this))
                    {
                        PathManager.SelectPath(this);
                    }
                    else
                    {
                        PathManager.PlacePoint(this, transform.position);
                    }
                }
            }
        }
    }

    protected override void OnUnselectObject()
    {
        Debug.Log("Unselect Pathpoint");
    }

    //UnityEvent Feedback ?
    protected override void InfrastructureOnMouseOver()
    {
       
    }

    /*private void OnMouseOver()
    {
        Debug.Log("testOver");
    }

    private void OnMouseExit()
    {
        Debug.Log("testExit");
    }*/
}
