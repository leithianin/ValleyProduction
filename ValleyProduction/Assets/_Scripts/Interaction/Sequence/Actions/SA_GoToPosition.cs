using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SA_GoToPosition : InteractionActions
{
    [SerializeField] private List<Transform> pathToTake;

    InteractionHandler inte;

    protected override void OnPlayAction(InteractionHandler caller)
    {
        if (caller.Movement != null)
        {
            List<Vector3> vectorPath = new List<Vector3>();

            for(int i = 0; i < pathToTake.Count; i++)
            {
                vectorPath.Add(pathToTake[i].position);
            }

            inte = caller;
            caller.Movement.WalkOnNewPath(vectorPath, () => EndAction(caller));
            //caller.onMovementEnd += EndAction;
        }
    }

    protected override void OnEndAction(InteractionHandler caller)
    {
        //caller.onMovementEnd -= EndAction;
    }

    private void EndWalk()
    {
        EndAction(inte);
    }
}
