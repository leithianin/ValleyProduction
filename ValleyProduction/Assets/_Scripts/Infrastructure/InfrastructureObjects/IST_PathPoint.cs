using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IST_PathPoint : Infrastructure
{
    [SerializeField] private PathNode node;

    public Action OnDestroyPathPoint;

    public PathNode Node => node;

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

        if (PathManager.IsSpawnPoint(this))                             //Si c'est le spawnPoint (boucle)
        {
            PathManager.PlacePoint(this, transform.position);
            PathManager.CreatePathData();
            UIManager.HideRoadsInfo();
        }
        else                                                                //Creer un nouveau chemin
        {
            //Check si le path est disconnected
            if (!PathManager.IsDeconnected(this))
            {
                //Need to check le sens
                PathManager.PlacePoint(this, transform.position);
            }
        }
    }

    protected override bool OnRemoveObject()
    {
        if (PathManager.HasManyPath(this))
        {
            node.UpdateNode();
            UIManager.ArrangePathButton(this);
            return false;
        }
        else
        {
            node.DeleteNode();

            PathManager.DeletePoint(this);

            InfrastructureManager.DesnapInfrastructure(this);

            Debug.Log("Remove object");
            OnDestroyPathPoint?.Invoke();
            return true;
        }
    }

    //Remove à partir de l'UI
    public void Remove(PathData pd)
    {
        node.DeleteNode();

        OnDestroyPathPoint?.Invoke();
        PathManager.DeletePoint(this, pd);
        InfrastructureManager.DesnapInfrastructure(this);
    }

    protected override void OnMoveObject()
    {
        PathManager.UpdateLineWhenMoving(this);
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
