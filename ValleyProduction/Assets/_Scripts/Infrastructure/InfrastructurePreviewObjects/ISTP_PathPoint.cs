using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ISTP_PathPoint : InfrastructurePreview
{
    [SerializeField] protected float maxDistance = 20f;
    [SerializeField] protected Vector2 snapRange = new Vector2(19.5f, 22.5f);

    protected override void OnAskToPlace(Vector3 position)
    {
        PathCreationManager.CalculatePathShortness(true);
    }

    protected override bool OnCanPlaceObject(Vector3 position)
    {
        float pathLength = PathCreationManager.CalculatePathShortness(false);

        if (PathManager.previousPathpoint == null || (pathLength < snapRange.y) && (doesSnap || pathLength > 1f))
        {
            if (PathManager.CurrentLinePreview != null)
            {
                PathManager.CurrentLinePreview.material.EnableKeyword("CAN_CONSTRUCT");
            }
            return true;
        }
        if (PathManager.CurrentLinePreview != null)
        {
            PathManager.CurrentLinePreview.material.DisableKeyword("CAN_CONSTRUCT");
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
