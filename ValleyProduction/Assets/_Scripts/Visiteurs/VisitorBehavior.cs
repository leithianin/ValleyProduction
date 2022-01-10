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

    [SerializeField] private List<TestPath> paths;
    [SerializeField] private CPN_Movement movement;

    private IST_PathPoint spawnPoint;
    private IST_PathPoint lastPoint;
    private IST_PathPoint currentPoint;

    public VisitorScriptable visitorType;

    private AnimationHandler visitorDisplay;

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

    public void StopWalk()
    {
        movement.StopWalk();
    }

    public void ContinueWalk()
    {
        movement.WalkOnCurrentPath();
    }

    // TEMPORAIRE
    private TestPath SearchPath()
    {
        return paths[UnityEngine.Random.Range(0, paths.Count)];
    }
}
