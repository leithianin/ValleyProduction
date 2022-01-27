using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SA_GoToPosition : InteractionActions
{
    [SerializeField] private List<Transform> pathToTake;

    protected override void OnPlayAction(InteractionHandler caller)
    {
        if (caller.Movement != null)
        {
            List<Vector3> vectorPath = new List<Vector3>();

            for(int i = 0; i < pathToTake.Count; i++)
            {
                vectorPath.Add(pathToTake[i].position);
            }

            caller.Movement.WalkOnNewPath(vectorPath, () => EndAction(caller));
        }
    }

    protected override void OnEndAction(InteractionHandler caller)
    {
        
    }

    protected override void OnInteruptAction(InteractionHandler caller)
    {
        caller.Movement.InteruptWalk();
    }
}
