using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IST_BaseStructure : Infrastructure
{
    protected override void InfrastructureOnMouseExit()
    {
        
    }

    protected override void InfrastructureOnMouseOver()
    {
        
    }

    protected override void OnHoldRightClic()
    {
        
    }

    protected override void OnMoveObject()
    {
        
    }

    protected override void OnPlaceObject(Vector3 position)
    {
        
    }

    protected override void OnPlaceObject()
    {
        
    }

    protected override bool OnRemoveObject()
    {
        return true;
    }

    protected override void OnReplaceObject()
    {
        
    }

    protected override void OnSelectObject()
    {
        UIManager.InteractWithInfrastructure(Data);
    }

    protected override void OnUnselectObject()
    {
        
    }
}
