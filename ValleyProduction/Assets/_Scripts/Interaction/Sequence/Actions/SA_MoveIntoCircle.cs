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

    private void Start()
    {
        circleCenter.position = new Vector3(circleCenter.position.x, VisitorManager.GetMainTerrain.SampleHeight(circleCenter.position) + 5f, circleCenter.position.z);
    }

    protected override void OnPlayAction(CPN_InteractionHandler caller)
    {
        Vector3 randomDirection = Random.insideUnitCircle * circleRadius;
        Vector3 randomPosition = circleCenter.position + new Vector3(randomDirection.x, 0, randomDirection.y);

        if (Vector3.Distance(caller.transform.position, randomPosition) > maxDistanceByMovement)
        {
            randomDirection = (randomPosition - caller.transform.position).normalized;
            randomPosition = circleCenter.position + randomDirection * maxDistanceByMovement;
        }
        else if(Vector3.Distance(caller.transform.position, randomPosition) <= 0.5f)
        {
            randomDirection = (randomPosition - caller.transform.position).normalized;
            randomPosition = circleCenter.position + randomDirection * 1f;
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

            CPN_Movement movement = null;
            if (caller.HasComponent<CPN_Movement>(ref movement))
            {
                movement.WalkOnNewPath(path, () => EndAction(caller));
            }
        }
        else
        {
            //Debug.Log("No path");
            TimerManager.CreateGameTimer(Time.deltaTime * 2f, () => EndAction(caller));
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
