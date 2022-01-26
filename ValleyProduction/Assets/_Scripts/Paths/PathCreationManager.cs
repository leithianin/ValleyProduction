using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathCreationManager : MonoBehaviour
{
    public GameObject Debug;
    public List<GameObject> DebugList;

    public static List<Vector3> navmeshPositionsList = new List<Vector3>();
    private NavMeshPath navPath;
    public int index = 1;

    private Vector3 offsetPathCalcul = new Vector3(0f, 0f, 0.5f); // -0.5f en z

    private void Awake()
    {
        navPath = new NavMeshPath();
    }

    // Update is called once per frame
    void Update()
    {
        if(PathManager.GetInstance.currentLineDebug)
        {
            LineRenderer lineRenderer = PathManager.GetInstance.currentLineDebug;
            navPath = new NavMeshPath();

            NavMesh.CalculatePath(PathManager.previousPathpoint.transform.position + offsetPathCalcul, PlayerInputManager.GetMousePosition + offsetPathCalcul, NavMesh.AllAreas, navPath);
            //lineRenderer.positionCount = index+1;
            //lineRenderer.SetPosition(index, PlayerInputManager.GetMousePosition + offsetPathCalcul);
            List<Vector3> points = new List<Vector3>();

            int j = 1;

            while(j < navPath.corners.Length)
            {
                //lineRenderer.positionCount = navPath.corners.Length;
                points = new List<Vector3>(navPath.corners);

                for(int k = 0; k < points.Count; k++)
                {
                    //lineRenderer.SetPosition(k, points[k]);
                }

                j++;
            }

            if(j == navPath.corners.Length && navPath.status == NavMeshPathStatus.PathComplete)
            {
                navmeshPositionsList = new List<Vector3>(points);
            }

            //DebugNavmesh();
            //index = lineRenderer.positionCount -1;

            lineRenderer.positionCount = 0;
            foreach(Vector3 vec in navmeshPositionsList)
            {
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, vec);
            }
        }
    }

    public void DebugNavmesh()
    {
        foreach(GameObject go in DebugList)
        {
            Destroy(go);
        }
        DebugList.Clear();

        foreach(Vector3 vec in navmeshPositionsList)
        {
            GameObject newGo = Instantiate(Debug, vec, Quaternion.identity);
            DebugList.Add(newGo);
        }
    }
}
