using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class CPN_Movement : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;

    // Is the visitor walking ?
    private bool isWalking;

    // A list of all the position the visitor will reach before reaching its destination.
    [HideInInspector] public List<Vector3> pathToTake = new List<Vector3>();

    private int currentPathIndex;
    private int interuptedPathIndex;

    [Header("Movement Events")]
    public UnityEvent OnStartWalking;
    public UnityEvent OnStopWalking;
    public UnityEvent OnEndWalking;

    // Update is called once per frame
    void Update()
    {
        if (agent.enabled && !agent.pathPending && isWalking)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                ReachDestination();
            }
        }
    }

    public void SetSpeed(float speed)
    {
        agent.speed = speed;
    }

    public void ContinueOnInteruptedPath()
    {
        StartWalk();
    }

    public void WalkOnNewPath(List<Vector3> nPathToTake)
    {
        pathToTake = new List<Vector3>(nPathToTake);

        foreach(Vector3 v in pathToTake)
        {
            Debug.Log(v);
        }

        currentPathIndex = 0;

        StartWalk();
    }

    private void StartWalk()
    {
        if (!isWalking)
        {
            isWalking = true;
            agent.isStopped = false;
            enabled = true;

            SetNextDestination(currentPathIndex);
            OnStartWalking?.Invoke();
        }
    }

    private void SetNextDestination(int pathIndex)
    {
        Vector3 targetPosition = transform.position;
        if (pathToTake.Count > pathIndex)
        {
            targetPosition = pathToTake[pathIndex];
        }

        if (Vector3.Distance(transform.position, targetPosition) <= 2f)
        {
            ReachDestination();
        }
        else
        {
            Vector3 randomPosition = pathToTake[pathIndex] + UnityEngine.Random.insideUnitSphere * 2f;

            agent.destination = randomPosition;
            StartWalk();
        }
    }

    private void ReachDestination()
    {
        currentPathIndex++;
        if (currentPathIndex < pathToTake.Count)
        {
            SetNextDestination(currentPathIndex);
        }
        else
        {
            StopWalk();

            OnEndWalking?.Invoke();
        }
    }

    public List<Vector3> InteruptWalk()
    {
        List<Vector3> toReturn = new List<Vector3>();

        if(isWalking)
        {
            interuptedPathIndex = currentPathIndex;

            for(int i = currentPathIndex; i < pathToTake.Count; i++)
            {
                toReturn.Add(pathToTake[i]);
            }

            StopWalk();
        }

        return toReturn;
    }

    private void StopWalk()
    {
        if(isWalking && enabled)
        {
            isWalking = false;
            agent.isStopped = true;
            OnStopWalking?.Invoke();
        }
    }
}
