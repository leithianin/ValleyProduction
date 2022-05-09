using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ISTP_PathPoint : InfrastructurePreview
{
    [SerializeField] protected float maxDistance = 20f;
    [SerializeField] protected Vector2 snapRange = new Vector2(19.5f, 22.5f);

    protected override void OnAskToPlace(Vector3 position)
    {
        //Debug.Log("Ask Pathpoint preview");
        PathCreationManager.CalculatePathShortness(true);
    }

    protected override bool OnCanPlaceObject(Vector3 position)
    {
        if (PathManager.previousPathpoint == null || PathCreationManager.CalculatePathShortness(false) < snapRange.y)
        {
            return true;
        }
        return false;
    }

    public override Vector3 TrySetPosition()
    {
        if (PathManager.previousPathpoint != null && PathCreationManager.CalculatePathShortness(true) >= snapRange.x && PathCreationManager.CalculatePathShortness(true) <= snapRange.y)
        {
            return PathCreationManager.CalculatePathWithMaxLength(maxDistance);
        }
        return base.TrySetPosition();
    }
}
