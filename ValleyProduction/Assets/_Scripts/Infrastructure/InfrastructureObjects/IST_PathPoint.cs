using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IST_PathPoint : Infrastructure
{
    public Action OnDestroyPathPoint;

    //Place on Terrain
    protected override void OnPlaceObject(Vector3 position)
    {
        PathManager.PlacePoint(this, position);
    }

    //Place on Click Infrastructure
    protected override void OnPlaceObject()
    {
        //Si c'est le dernier PathPoint du chemin = Terminer chemin
        if (this == PathManager.previousPathpoint)
        {
            PathManager.CreatePathData();
            UIManager.HideRoadsInfo();
            return;
        }
        else
        {
            PathManager.PlacePoint(this, transform.position);
        }
    }

    protected override void OnRemoveObject()
    {
        PathManager.DeletePoint(this);
        /*if (PathManager.CanDeleteGameobject(this))
        {
            OnDestroyPathPoint?.Invoke();
            return true;
        
        return false;*/
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
        if (PathManager.HasManyPath(this)) {UIManager.ArrangePathButton(this)                          ;}
        else                               {UIManager.ShowRoadsInfos(PathManager.GetPathData(this))    ;}

        /*
        if (!PathManager.IsPathpointListEmpty())
            {
                PathManager.PlacePoint(this, transform.position);
                if(PathManager.IsSpawnPoint(this))
                {
                    PathManager.CreatePathData();
                    UIManager.HideRoadsInfo();
                }
            }
            else
            {
                //Check si plusieurs PathData    
            }
        */
    }

    protected override void OnUnselectObject()
    {
        Debug.Log("Unselect Pathpoint");
    }

    //UnityEvent Feedback ?
    protected override void InfrastructureOnMouseOver()
    {
       
    }

    protected override void InfrastructureOnMouseExit()
    {
        
    }
}
