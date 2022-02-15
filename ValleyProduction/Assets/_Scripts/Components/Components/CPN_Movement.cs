using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class CPN_Movement : VLY_Component<CPN_Data_Movement>
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
    /// Set la vitesse de d�placement.
    /// </summary>
    /// <param name="speed">La nouvelle vitesse de d�placement.</param>
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
    /// Demande � se d�placer selon un chemin d�finit, puis joue une Action � la fin du d�placement.
    /// </summary>
    /// <param name="nPathToTake">Le chemin � prendre.</param>
    /// <param name="callback">L'Action qui sera effectu�e � la fin du d�placement.</param>
    public void WalkOnNewPath(List<Vector3> nPathToTake, Action callback)
    {
        //Debug.Log("Add Callback");
        reachDestinationCallback += callback;

        pathToTake = new List<Vector3>(nPathToTake);

        currentPathIndex = 0;

        StartWalk();
    }

    /// <summary>
    /// Demande � se d�placer selon un chemin d�finit.
    /// </summary>
    /// <param name="nPathToTake">Le chemin � prendre.</param>
    public void WalkOnNewPath(List<Vector3> nPathToTake)
    {
        reachDestinationCallback = null;

        pathToTake = new List<Vector3>(nPathToTake);

        currentPathIndex = 0;

        StartWalk();
    }

    /// <summary>
    /// Demande au personnage de commencer � parcourir son chemin.
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
    /// Demande de se d�placer vers la prochaine destination.
    /// </summary>
    /// <param name="pathIndex">L'index de la position vis�e sur la liste du chemin � parcourir.</param>
    private void SetNextDestination(int pathIndex)
    {
        Vector3 targetPosition = transform.position;
        if (pathToTake.Count > pathIndex)
        {
            targetPosition = pathToTake[pathIndex];
        }

        if (Vector3.Distance(transform.position, targetPosition) <= agent.stoppingDistance)
        {
            Debug.Log("Close to current Pos");
            ReachDestination();
        }
        else
        {
            Vector3 randomPosition = pathToTake[pathIndex] + UnityEngine.Random.insideUnitSphere * 2f;

            agent.destination = randomPosition;
        }
    }

    /// <summary>
    /// Appel� quand la destination est atteinte. V�rifie si le chemin est finit ou non.
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

            //Debug.Log("Reach Destination : " + Time.time);
            reachDestinationCallback?.Invoke();

            PlayOnEndWalking?.Invoke();
        }
    }

    /// <summary>
    /// Int�rromp le d�placement.
    /// </summary>
    /// <returns>La liste des points qu'il lui reste � faire pour finir son d�placement.</returns>
    public List<Vector3> InteruptWalk()
    {
        //Debug.Log("Remove Callback");
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
    /// Fait s'arr�ter le personnage.
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

    public override void SetData(CPN_Data_Movement dataToSet)
    {
        SetSpeed(dataToSet.Speed());
    }
}
