using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathCreationManager : VLY_Singleton<PathCreationManager>
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
            CalculateCurrentPath();
        }
    }

    //Calculate Path each frame you're editing a path
    public void CalculateCurrentPath()
    {
        navPath = new NavMeshPath();

        if (PathManager.previousPathpoint != null)
        {
            NavMesh.CalculatePath(PathManager.previousPathpoint.transform.position + offsetPathCalcul, PlayerInputManager.GetMousePosition + offsetPathCalcul, NavMesh.AllAreas, navPath);
        }


        List<Vector3> points = new List<Vector3>();

        int j = 1;

        while (j < navPath.corners.Length)
        {
            points = new List<Vector3>(navPath.corners);
            j++;
        }

        if (j == navPath.corners.Length && navPath.status == NavMeshPathStatus.PathComplete)
        {
            navmeshPositionsList = new List<Vector3>(points);
        }

        //DebugNavmesh();

        ShowPathLine(navmeshPositionsList);
    }

    public void CalculatePath(IST_PathPoint ppstart, IST_PathPoint ppend, PathFragmentData pathFrag)
    {
        NavMesh.CalculatePath(ppstart.transform.position + offsetPathCalcul, ppend.transform.position + offsetPathCalcul, NavMesh.AllAreas, navPath);

        List<Vector3> points = new List<Vector3>();

        int j = 1;

        while (j < navPath.corners.Length)
        {
            points = new List<Vector3>(navPath.corners);
            j++;
        }

        if (j == navPath.corners.Length && navPath.status == NavMeshPathStatus.PathComplete)
        {
            navmeshPositionsList = new List<Vector3>(points);
        }

        foreach (Vector3 vec in navmeshPositionsList)
        {
            pathFrag.path.Add(vec);
        }
    }

    //Calculate Path after moving
    public void UpdateLineRendererAfterMoving(List<PathFragmentData> pfdList)
    {
        //Je reçois la liste 

        foreach(PathFragmentData pfd in pfdList)
        {
            CalculatePath(pfd.startPoint, pfd.endPoint, pfd);
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

    public void ShowPathLine(List<Vector3> path)
    {
        LineRenderer lineRenderer = PathManager.GetInstance.currentLineDebug;

        lineRenderer.positionCount = 0;
        foreach (Vector3 vec in path)
        {
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, vec);
        }
    }
}
