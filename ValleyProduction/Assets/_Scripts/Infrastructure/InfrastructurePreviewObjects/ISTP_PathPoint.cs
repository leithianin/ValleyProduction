using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ISTP_PathPoint : InfrastructurePreview
{
    [SerializeField] private float maxDistance = 20f;

    protected override void OnAskToPlace(Vector3 position)
    {
        //Debug.Log("Ask Pathpoint preview");
    }

    protected override bool OnCanPlaceObject(Vector3 position)
    {
        if (PathManager.previousPathpoint == null || Vector3.Distance(position, PathManager.previousPathpoint.transform.position) <= maxDistance)
        {
            return true;
        }
        return false;
    }
}
