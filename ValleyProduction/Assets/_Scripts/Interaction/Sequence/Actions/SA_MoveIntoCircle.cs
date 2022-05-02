using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SA_MoveIntoCircle : InteractionActions
{
    [SerializeField] private Transform circleCenter;
    [SerializeField] private float circleRadius;
    [SerializeField, Tooltip("Détermine la distance minimale et maximal d'un déplacement.")] private Vector2 rangeDistanceByMovement;

    NavMeshPath pathToTake;

    private void Start()
    {
        NavMeshHit hit;
        NavMesh.SamplePosition(circleCenter.position, out hit, 10000f, NavMesh.AllAreas);
        circleCenter.position = hit.position;
    }

    protected override void OnPlayAction(CPN_InteractionHandler caller)
    {
        Vector3 randomDirection = Random.insideUnitCircle * circleRadius;
        Vector3 randomPosition = circleCenter.position + new Vector3(randomDirection.x, 0, randomDirection.y);

        if (Vector3.Distance(caller.transform.position, randomPosition) > rangeDistanceByMovement.y)
        {
            randomDirection = (randomPosition - caller.transform.position).normalized;
            randomPosition = circleCenter.position + randomDirection * rangeDistanceByMovement.y;
        }
        else if(Vector3.Distance(caller.transform.position, randomPosition) < rangeDistanceByMovement.x)
        {
            randomDirection = (randomPosition - caller.transform.position).normalized;
            randomPosition = circleCenter.position + randomDirection * rangeDistanceByMovement.x;
        }

        NavMeshHit hit;
        NavMesh.SamplePosition(randomPosition, out hit, 10000f, NavMesh.AllAreas);
        randomPosition = hit.position;

        pathToTake = new NavMeshPath();

        if (NavMesh.CalculatePath(caller.transform.position, randomPosition, NavMesh.AllAreas, pathToTake))
        {
            List<Vector3> path = new List<Vector3>();

            for (int i = 0; i < pathToTake.corners.Length; i++)
            {
                path.Add(pathToTake.corners[i]);
            }

            if (caller.HasComponent<CPN_Movement>(out CPN_Movement movement))
            {
                movement.WalkOnNewPath(path, () => EndAction(caller));
            }
        }
        else
        {
            Debug.Log("No path");
            TimerManager.CreateGameTimer(Time.deltaTime * 2f, () => EndAction(caller));
        }
    }

    protected override void OnEndAction(CPN_InteractionHandler caller)
    {
        
    }

    protected override void OnInteruptAction(CPN_InteractionHandler caller)
    {
        if (caller.HasComponent<CPN_Movement>(out CPN_Movement movement))
        {
            movement.InteruptWalk();
        }
    }
}
