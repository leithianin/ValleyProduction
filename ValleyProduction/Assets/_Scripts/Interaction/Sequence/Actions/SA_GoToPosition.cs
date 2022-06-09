using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SA_GoToPosition : InteractionActions
{
    [SerializeField] private List<Transform> pathToTake;

    protected override void OnPlayAction(CPN_InteractionHandler caller)
    {
        if (caller.HasComponent<CPN_Movement>(out CPN_Movement movement))
        {
            List<Vector3> vectorPath = new List<Vector3>();

            for(int i = 0; i < pathToTake.Count; i++)
            {
                vectorPath.Add(pathToTake[i].position);
            }

            movement.Agent.stoppingDistance = 0.1f;
            movement.WalkOnNewPath(vectorPath, () => EndAction(caller), 0);
        }
        else
        {
            EndAction(caller);
        }
    }

    protected override void OnEndAction(CPN_InteractionHandler caller)
    {
        if (caller.HasComponent<CPN_Movement>(out CPN_Movement movement))
        {
            movement.Agent.stoppingDistance = 1f;
        }
    }

    protected override void OnInteruptAction(CPN_InteractionHandler caller)
    {
        if (caller.HasComponent<CPN_Movement>(out CPN_Movement movement))
        {
            movement.Agent.stoppingDistance = 1f;
            movement.InteruptWalk();
        }
    }
}
