using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SA_GoToPosition : InteractionActions
{
    [SerializeField] private List<Transform> pathToTake;

    protected override void OnPlayAction(CPN_InteractionHandler caller)
    {
        CPN_Movement movement = null;
        if (caller.HasComponent<CPN_Movement>(ref movement))
        {
            List<Vector3> vectorPath = new List<Vector3>();

            for(int i = 0; i < pathToTake.Count; i++)
            {
                vectorPath.Add(pathToTake[i].position);
            }

            movement.WalkOnNewPath(vectorPath, () => EndAction(caller), 0);
        }
        else
        {
            EndAction(caller);
        }
    }

    protected override void OnEndAction(CPN_InteractionHandler caller)
    {
        
    }

    protected override void OnInteruptAction(CPN_InteractionHandler caller)
    {
        CPN_Movement movement = null;
        if (caller.HasComponent<CPN_Movement>(ref movement))
        {
            movement.InteruptWalk();
        }
    }
}
