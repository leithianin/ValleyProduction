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

    private void Start()
    {
        SearchDestination();
    }

    public void SetVisitor(IST_PathPoint nSpawnPoint, Vector3 spawnPosition)
    {
        TestPath wantedPath = SearchPath();

        if(wantedPath != null)
        {
            // Type de visiteur
            // Stats de visiteur

            movement.SetSpeed(3f);

            spawnPoint = nSpawnPoint;
            lastPoint = nSpawnPoint;
            currentPoint = nSpawnPoint;

            transform.position = spawnPosition;

            gameObject.SetActive(true);

            // Gestion de l'affichage du visiteur

            List<Vector3> vectPath = new List<Vector3>();
            foreach (Transform t in wantedPath.pathPoints)
            {
                vectPath.Add(t.position);
            }

            movement.AsktoStartWalk(vectPath);
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

        movement.AsktoStartWalk(vectPath);
    }

    public void ReachDestination()
    {
        // Check si despawn ou autre
        SearchDestination();
    }

    private TestPath SearchPath()
    {
        return paths[UnityEngine.Random.Range(0, paths.Count)];
    }
}
