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
                Destroy(visitorDisplay);
            }

            visitorDisplay = Instantiate(visitorType.Display, transform);

            movement.WalkOnNewPath(currentPathFragment.path);
        }
    }

    public void UnsetVisitor()
    {
        currentPathFragment = null;

        gameObject.SetActive(false);
    }

    public void SearchDestination()
    {
        currentPathFragment = SearchNextPathFragment();

        movement.WalkOnNewPath(currentPathFragment.path);
    }

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

    public void InteruptWalk()
    {
        movement.PlayOnEndWalking.RemoveListener(ReachDestination);
        interuptedPath = new List<Vector3>(movement.InteruptWalk());
    }

    public void ContinueWalk()
    {
        movement.PlayOnEndWalking.AddListener(ReachDestination);
        movement.WalkOnNewPath(interuptedPath);
    }

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

    public void PlayMovementAnimation()
    {
        visitorDisplay.PlayBodyAnim(BodyAnimationType.Walk);
    }

    public void StopMovementAnimation()
    {
        visitorDisplay.StopBodyAnim(BodyAnimationType.Walk);
    }
}
