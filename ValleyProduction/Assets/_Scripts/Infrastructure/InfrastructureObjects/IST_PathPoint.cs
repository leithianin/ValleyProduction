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
        PathManager.PlacePoint(this);
    }

    //Place on Click Infrastructure
    protected override void OnPlaceObject()
    {
        //Si c'est le dernier PathPoint du chemin = Terminer chemin
        if (this == PathManager.previousPathpoint)
        {
            PathManager.CreatePathData();
            UIManager.HideShownGameObject();
            return;
        }

        if (PathManager.IsSpawnPoint(this))                             //Si c'est le spawnPoint (boucle)
        {
            PathManager.PlacePoint(this);
            PathManager.CreatePathData();
            UIManager.HideShownGameObject();
        }
        else                                                                //Creer un nouveau chemin
        {
            PathManager.PlacePoint(this);
        }
    }

    protected override bool OnRemoveObject()
    {
        if (PathManager.HasManyPath(this))
        {
            UIManager.ArrangePathButton(this);
            return false;
        }
        else
        {
            node.DeleteNode();
            if (PathManager.GetCurrentPathpointList.Count > 0)
            {
                PathManager.UnplacePoint(this);
            }
            else if (PathManager.GetCurrentPathData == null)
            {
                PathManager.DeletePoint(this);
            }

            InfrastructureManager.DesnapInfrastructure(this);
            Debug.Log("Remove object");
            OnDestroyPathPoint?.Invoke();
            return true;
        }
    }

    //Remove � partir de l'UI
    public void Remove(PathData pd)
    {
        node.DeleteNode();

        OnDestroyPathPoint?.Invoke();
        PathManager.DeletePoint(this, pd);
        InfrastructureManager.DesnapInfrastructure(this);
    }

    protected override void OnStartMoveObject()
    {
        PathManager.StartMovingPoint(this);
    }


    protected override void OnMoveObject()
    {
        PathManager.UpdateLineWhenMoving(this);
    }

    protected override void OnReplaceObject()
    {
        node.PlaceNode();
        PathManager.UpdateAfterMoving(this);
    }

    //Pas sur de ce que je fais l�
    [Obsolete]
    protected override void OnHoldRightClic()
    {
        PathManager.CreatePathData();
        PathManager.PlacePoint(this);
        UIManager.HideShownGameObject();
    }

    protected override void OnSelectObject()
    {
        UIManager.InteractWithObject(gameObject) ;
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
