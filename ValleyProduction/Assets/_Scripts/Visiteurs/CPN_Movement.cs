using System;
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
    public List<Vector3> pathToTake = new List<Vector3>();

    private int currentPathIndex;

    [Header("Movement Events")]
    public UnityEvent PlayOnStartWalking;
    public UnityEvent PlayOnStopWalking;
    public UnityEvent PlayOnEndWalking;

    private Action reachDestinationCallback;

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

    private void OnDisable()
    {
        //InteruptWalk();
    }

    /// <summary>
    /// Set la vitesse de déplacement.
    /// </summary>
    /// <param name="speed">La nouvelle vitesse de déplacement.</param>
    public void SetSpeed(float speed)
    {
        agent.speed = speed;
    }

    /// <summary>
    /// Reprend sont chemin.
    /// </summary>
    public void ContinueOnInteruptedPath()
    {
        StartWalk();
    }

    /// <summary>
    /// Demande à se déplacer selon un chemin définit, puis joue une Action à la fin du déplacement.
    /// </summary>
    /// <param name="nPathToTake">Le chemin à prendre.</param>
    /// <param name="callback">L'Action qui sera effectuée à la fin du déplacement.</param>
    public void WalkOnNewPath(List<Vector3> nPathToTake, Action callback)
    {
        Debug.Log("Add Callback");
        reachDestinationCallback += callback;

        pathToTake = new List<Vector3>(nPathToTake);

        currentPathIndex = 0;

        StartWalk();
    }

    /// <summary>
    /// Demande à se déplacer selon un chemin définit.
    /// </summary>
    /// <param name="nPathToTake">Le chemin à prendre.</param>
    public void WalkOnNewPath(List<Vector3> nPathToTake)
    {
        Debug.Log("Remove Callback");
        reachDestinationCallback = null;

        pathToTake = new List<Vector3>(nPathToTake);

        currentPathIndex = 0;

        StartWalk();
    }

    /// <summary>
    /// Demande au personnage de commencer à parcourir son chemin.
    /// </summary>
    private void StartWalk()
    {
        if (!isWalking)
        {
            isWalking = true;
            agent.isStopped = false;
            enabled = true;

            SetNextDestination(currentPathIndex);
            PlayOnStartWalking?.Invoke();
        }
    }
    
    /// <summary>
    /// Demande de se déplacer vers la prochaine destination.
    /// </summary>
    /// <param name="pathIndex">L'index de la position visée sur la liste du chemin à parcourir.</param>
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

    /// <summary>
    /// Appelé quand la destination est atteinte. Vérifie si le chemin est finit ou non.
    /// </summary>
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

            Debug.Log("Reach Destination");
            reachDestinationCallback?.Invoke();

            PlayOnEndWalking?.Invoke();
        }
    }

    /// <summary>
    /// Intérromp le déplacement.
    /// </summary>
    /// <returns>La liste des points qu'il lui reste à faire pour finir son déplacement.</returns>
    public List<Vector3> InteruptWalk()
    {
        Debug.Log("Remove Callback");
        reachDestinationCallback = null;

        List<Vector3> toReturn = new List<Vector3>();

        if(isWalking)
        {
            for(int i = currentPathIndex; i < pathToTake.Count; i++)
            {
                toReturn.Add(pathToTake[i]);
            }

            StopWalk();
        }

        return toReturn;
    }

    /// <summary>
    /// Fait s'arrêter le personnage.
    /// </summary>
    private void StopWalk()
    {
        if(isWalking && enabled)
        {
            isWalking = false;
            agent.isStopped = true;
            PlayOnStopWalking?.Invoke();
        }
    }
}
