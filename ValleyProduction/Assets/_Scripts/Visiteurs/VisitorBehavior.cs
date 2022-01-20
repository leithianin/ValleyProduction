using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VisitorBehavior : MonoBehaviour
{
    [Serializable]
    public class TestPath
    {
        public List<Transform> pathPoints;
    }

    public int testInt;

    public int TestInt => testInt;

    [SerializeField] private List<TestPath> paths;
    [SerializeField] private CPN_Movement movement;

    private IST_PathPoint spawnPoint;
    private IST_PathPoint lastPoint;
    private IST_PathPoint currentPoint;

    private VisitorScriptable visitorType;

    private AnimationHandler visitorDisplay;

    List<Vector3> interuptedPath = new List<Vector3>();

    [SerializeField] private UnityEvent<VisitorScriptable> OnSetVisitor;

    public CPN_Movement Movement => movement;

    public VisitorScriptable VisitorType => visitorType;

    private void Start()
    {
        movement.PlayOnEndWalking.AddListener(ReachDestination);
    }

    public void SetVisitor(IST_PathPoint nSpawnPoint, Vector3 spawnPosition, VisitorScriptable nVisitorType)
    {
        TestPath wantedPath = SearchPath();

        if(wantedPath != null)
        {
            visitorType = nVisitorType;

            movement.SetSpeed(visitorType.Speed);

            spawnPoint = nSpawnPoint;
            lastPoint = nSpawnPoint;
            currentPoint = nSpawnPoint;

            transform.position = spawnPosition;

            gameObject.SetActive(true);

            if(visitorDisplay != null)
            {
                Destroy(visitorDisplay);
            }

            visitorDisplay = Instantiate(visitorType.Display, transform);

            SearchDestination();

            OnSetVisitor?.Invoke(visitorType);
        }
    }

    public void UnsetVisitor()
    {
        currentPoint = null;

        gameObject.SetActive(false);
    }

    public void SearchDestination()
    {
        // Récupération du prochain chemin

        List<Vector3> vectPath = new List<Vector3>();
        List<Transform> wantedPath = SearchPath().pathPoints;
        foreach (Transform t in wantedPath)
        {
            vectPath.Add(t.position);
        }

        movement.WalkOnNewPath(vectPath);
    }

    public void ReachDestination()
    {
        // Check si despawn ou autre

        if (Vector3.Distance(spawnPoint.transform.position, transform.position) < 2f)
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

    // TEMPORAIRE
    private TestPath SearchPath()
    {
        return paths[UnityEngine.Random.Range(0, paths.Count)];
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
