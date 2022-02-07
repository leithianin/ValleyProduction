using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SA_MoveIntoCircle : InteractionActions
{
    [SerializeField] private Transform circleCenter;
    [SerializeField] private float circleRadius;
    [SerializeField] private float maxDistanceByMovement;

    NavMeshPath pathToTake;

    protected override void OnPlayAction(InteractionHandler caller)
    {
        Vector3 randomDirection = Random.insideUnitCircle * circleRadius;
        Vector3 randomPosition = circleCenter.position + new Vector3(randomDirection.x, 0, randomDirection.y);

        if (Vector3.Distance(caller.transform.position, randomPosition) > maxDistanceByMovement)
        {
            randomDirection = (randomPosition - caller.transform.position).normalized;
            randomPosition = circleCenter.position + randomDirection * maxDistanceByMovement;
        }

        NavMeshHit hit;
        NavMesh.SamplePosition(randomPosition, out hit, circleRadius, NavMesh.AllAreas);
        randomPosition = hit.position;

        pathToTake = new NavMeshPath();

        if (NavMesh.CalculatePath(caller.transform.position, randomPosition, NavMesh.AllAreas, pathToTake))
        {

            List<Vector3> path = new List<Vector3>();

            for (int i = 0; i < pathToTake.corners.Length; i++)
            {
                path.Add(pathToTake.corners[i]);
            }
            
            caller.Movement.WalkOnNewPath(path, () => EndAction(caller));
        }
        else
        {
            Debug.Log("No path");
            StartCoroutine(EndNotPath(caller));
        }
    }

    IEnumerator EndNotPath(InteractionHandler caller)
    {
        yield return new WaitForSeconds(Time.deltaTime);
        EndAction(caller);
    }

    protected override void OnEndAction(InteractionHandler caller)
    {
        
    }

    protected override void OnInteruptAction(InteractionHandler caller)
    {
        caller.Movement.InteruptWalk();
    }
}
