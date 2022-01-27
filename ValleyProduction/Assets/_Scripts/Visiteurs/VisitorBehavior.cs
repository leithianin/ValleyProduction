using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VisitorBehavior : MonoBehaviour
{
    [SerializeField] private CPN_Movement movement;

    private IST_PathPoint spawnPoint;
    private PathData currentPath;
    private PathFragmentData currentPathFragment = null;

    public VisitorScriptable visitorType;

    private AnimationHandler visitorDisplay;

    List<Vector3> interuptedPath = new List<Vector3>();

    public CPN_Movement Movement => movement;

    private void Start()
    {
        movement.PlayOnEndWalking.AddListener(ReachDestination);
    }

    /// <summary>
    /// Active le visiteur.
    /// </summary>
    /// <param name="nSpawnPoint">Le point de spawn du visiteur.</param>
    /// <param name="spawnPosition">La position de spawn du visiteur.</param>
    /// <param name="nVisitorType">Le type de visiteur voulut.</param>
    /// <param name="nPath">Le chemin choisit par le visiteur.</param>
    public void SetVisitor(IST_PathPoint nSpawnPoint, Vector3 spawnPosition, VisitorScriptable nVisitorType, PathData nPath)
    {
        currentPath = nPath;

        spawnPoint = nSpawnPoint;

        currentPathFragment = SearchFirstPathFragment(nSpawnPoint);

        if(currentPathFragment != null)
        {
            visitorType = nVisitorType;

            movement.SetSpeed(visitorType.Speed);

            transform.position = spawnPosition;

            gameObject.SetActive(true);

            if(visitorDisplay != null)
            {
                Destroy(visitorDisplay.gameObject);
            }

            visitorDisplay = Instantiate(visitorType.Display, transform);

            movement.WalkOnNewPath(currentPathFragment.path);
        }
    }

    /// <summary>
    /// D�sactive le visiteur.
    /// </summary>
    public void UnsetVisitor()
    {
        currentPathFragment = null;

        gameObject.SetActive(false);
    }

    /// <summary>
    /// Cherche le prochain Pathpoint � atteindre.
    /// </summary>
    public void SearchDestination()
    {
        currentPathFragment = SearchNextPathFragment();

        movement.WalkOnNewPath(currentPathFragment.path);
    }

    /// <summary>
    /// Appel� quand le visiteur atteint un PathPoint.
    /// </summary>
    public void ReachDestination()
    {
        // Check si despawn ou autre

        if (currentPathFragment.endPoint == spawnPoint)
        {
            VisitorManager.DeleteVisitor(this);
        }
        else
        {
            SearchDestination();
        }
    }

    /// <summary>
    /// Interromp le d�placement du visiteur.
    /// </summary>
    public void InteruptWalk()
    {
        movement.PlayOnEndWalking.RemoveListener(ReachDestination);
        interuptedPath = new List<Vector3>(movement.InteruptWalk());
    }

    /// <summary>
    /// Reprend le d�placement du visiteur o� il s'�tait arr�t� sur le chemin.
    /// </summary>
    public void ContinueWalk()
    {
        movement.PlayOnEndWalking.AddListener(ReachDestination);
        movement.WalkOnNewPath(interuptedPath);
    }

    /// <summary>
    /// Cherche le premier PathFragment � parcourir.
    /// </summary>
    /// <param name="startPoint">Le point de d�part du PathFragment recherch�.</param>
    /// <returns>Le PathFragment � parcourir.</returns>
    private PathFragmentData SearchFirstPathFragment(IST_PathPoint startPoint)
    {
        List<PathFragmentData> possibleNextFragment = new List<PathFragmentData>();

        for (int i = 0; i < currentPath.pathFragment.Count; i++)
        {
            if(currentPath.pathFragment[i].startPoint == startPoint)
            {
                possibleNextFragment.Add(currentPath.pathFragment[i]);
            }
            else if(currentPath.pathFragment[i].endPoint == startPoint)
            {
                possibleNextFragment.Add(new PathFragmentData(currentPath.pathFragment[i].endPoint, currentPath.pathFragment[i].startPoint, currentPath.pathFragment[i].GetReversePath()));
            }
        }

        if (possibleNextFragment.Count == 0)
        {
            return null;
        }

        return possibleNextFragment[UnityEngine.Random.Range(0, possibleNextFragment.Count)];
    }

    /// <summary>
    /// Cherche le prochain PathFragment � parcourir.
    /// </summary>
    /// <returns>Le PathFragment � parcourir.</returns>
    private PathFragmentData SearchNextPathFragment()
    {
        List<PathFragmentData> possibleNextFragment = new List<PathFragmentData>();
        
        for(int i = 0; i < currentPath.pathFragment.Count; i++)
        {
            int neighbourValue = currentPath.pathFragment[i].IsFragmentNeighbours(currentPathFragment);
            if (neighbourValue != 0 && !currentPath.pathFragment[i].IsSameFragment(currentPathFragment))
            {
                if(neighbourValue > 0)
                {
                    possibleNextFragment.Add(currentPath.pathFragment[i]);
                }
                else
                {
                    possibleNextFragment.Add(new PathFragmentData(currentPath.pathFragment[i].endPoint, currentPath.pathFragment[i].startPoint, currentPath.pathFragment[i].GetReversePath()));
                }
            }
        }

        if(possibleNextFragment.Count == 0)
        {
            if (currentPathFragment == null)
            {
                return null;
            }
            else
            {
                Debug.Log("Same fragment");
                possibleNextFragment.Add(new PathFragmentData(currentPathFragment.endPoint, currentPathFragment.startPoint, currentPathFragment.GetReversePath()));
            }
        }

        return possibleNextFragment[UnityEngine.Random.Range(0, possibleNextFragment.Count)];
    }

    /// <summary>
    /// Joue l'animation de mouvement.
    /// </summary>
    public void PlayMovementAnimation()
    {
        visitorDisplay.PlayBodyAnim(BodyAnimationType.Walk);
    }

    /// <summary>
    /// Stop l'animation de mouvement.
    /// </summary>
    public void StopMovementAnimation()
    {
        visitorDisplay.StopBodyAnim(BodyAnimationType.Walk);
    }
}
