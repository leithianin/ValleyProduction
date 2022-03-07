using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathCreationManager : VLY_Singleton<PathCreationManager>
{
    public GameObject debugObject;
    public List<GameObject> DebugList;

    public static List<Vector3> navmeshPositionsList = new List<Vector3>();
    private NavMeshPath navPath;
    public int index = 1;

    private Vector3 offsetPathCalcul = new Vector3(0f, 0f, 0.5f); // -0.5f en z

    [Header("Modify Button")]
    public static List<ModifyListClass> ModifyList = new List<ModifyListClass>();
    private List<AdditionalPathpointClass> additionalPathpointList = new List<AdditionalPathpointClass>();
    public static bool isModifyPath = false;
    private float distance = 0f;

    public GameObject testPathpoint;

    //List des pathFragment à modifier avec leur LineRenderer
    public class ModifyListClass 
    {
        public LineRenderer modifyLinesRenderer;
        public PathFragmentData modifyPathFragment;

        public ModifyListClass(LineRenderer modifyLinesRendererV, PathFragmentData modifyPathFragmentV)
        {
            modifyLinesRenderer = modifyLinesRendererV;
            modifyPathFragment = modifyPathFragmentV;
        }
    }

    public class AdditionalPathpointClass
    {
        public List<IST_PathPoint> pathpointList = new List<IST_PathPoint>();
    }

    public static void GetUpdateSeveralLine() => instance.UpdateSeveralLines();

    private void Awake()
    {
        instance = this;
        navPath = new NavMeshPath();
    }

    // Update is called once per frame
    void Update()
    {
        if(PathManager.GetInstance.currentLineDebug)
        {
            CalculateCurrentPath();
        }

        if(isModifyPath)
        {
            UpdateSeveralLines();
        }
    }

    //Calculate Path each frame you're editing a path
    public void CalculateCurrentPath()
    {
        navPath = new NavMeshPath();

        if (PathManager.previousPathpoint != null && InfrastructureManager.GetCurrentPreview != null)
        {
            NavMesh.CalculatePath(PathManager.previousPathpoint.transform.position + offsetPathCalcul, InfrastructureManager.GetCurrentPreview.transform.position + offsetPathCalcul, NavMesh.AllAreas, navPath);
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

    public void UpdateSeveralLines()
    {
        while(ModifyList.Count > additionalPathpointList.Count)
        {
            additionalPathpointList.Add(new AdditionalPathpointClass());
        }

        for (int i = 0; i < ModifyList.Count; i++)
        {
            //Check distance entre chaque point
            if (IsPathShortEnough(ModifyList[i].modifyPathFragment.startPoint, ModifyList[i].modifyPathFragment.endPoint))
            {
                //Debug.Log(i + " Proche");
                AddMarkers(distance, i);
            }
            else
            {
                AddMarkers(distance, i);
                //Debug.Log(i + " Loin");
            }

            navPath = new NavMeshPath();

            NavMesh.CalculatePath(ModifyList[i].modifyPathFragment.startPoint.transform.position + offsetPathCalcul, ModifyList[i].modifyPathFragment.endPoint.transform.position + offsetPathCalcul, NavMesh.AllAreas, navPath);

            List<Vector3> points = new List<Vector3>();

            int j = 1;

            while (j < navPath.corners.Length)
            {
                points = new List<Vector3>(navPath.corners);
                j++;
            }

            if (j == navPath.corners.Length && navPath.status == NavMeshPathStatus.PathComplete)
            {
                ModifyList[i].modifyPathFragment.path = new List<Vector3>(points);
                navmeshPositionsList = new List<Vector3>(points);
            }

            //DebugNavmesh();

            ShowPathLine(navmeshPositionsList, ModifyList[i].modifyLinesRenderer);
        }
    }

    public static bool IsPathShortEnough(float maxDistance)
    {
        float currentDistance = 0;

        for(int i = 0; i < navmeshPositionsList.Count - 1; i++)
        {
            currentDistance += Vector3.Distance(navmeshPositionsList[i], navmeshPositionsList[i + 1]);

            if(currentDistance > maxDistance)
            {
                return false;
            }
        }
        return true;
    }

    public static bool IsPathShortEnough(IST_PathPoint pp1, IST_PathPoint pp2)
    {
        float currentDistance = 0;

        currentDistance += Vector3.Distance(pp1.transform.position, pp2.transform.position);

        if (currentDistance > 20f)
        {
            instance.distance = currentDistance;
            return false;
        }

        instance.distance = currentDistance;
        return true;
    }

    public void AddMarkers(float distance, int index)
    {
        Debug.Log("Add markers");
        //Debug.Log(distance + " %20 = " + result);

        int result = (int)distance / 20;

        for (int i = 0; i < result; i++)
        {        
            if(i >= additionalPathpointList[index].pathpointList.Count)
            {
                additionalPathpointList[index].pathpointList.Add(new IST_PathPoint());
            }

            if (additionalPathpointList[index].pathpointList[i] == null)
            {
                Debug.Log("Instantiate new Pathpoint");
                GameObject pathpoint = Instantiate(testPathpoint, PlayerInputManager.GetMousePosition, Quaternion.identity);
                additionalPathpointList[index].pathpointList[i] = pathpoint.GetComponent<IST_PathPoint>();
                //Creer pathpoint
                //Add à la liste
                //Placer à l'endroit même ou est le joueur
                //Update line ? => Il faut qu'il parte du point qu'on place
            }
            else
            {
                Debug.Log("Existe déjà dans la liste");
                //Existe déjà dans la liste, update ?
            }
        }

        //Si il y'en a plus dans la liste qu'on en a parcouru dans le for --> Delete les points
        if (result < additionalPathpointList[index].pathpointList.Count)
        {
            for(int i = additionalPathpointList[index].pathpointList.Count; additionalPathpointList[index].pathpointList.Count > result; i--)
            {
                Destroy(additionalPathpointList[index].pathpointList[additionalPathpointList[index].pathpointList.Count-1].gameObject);
                additionalPathpointList[index].pathpointList.RemoveAt(additionalPathpointList[index].pathpointList.Count-1);
            }
        }
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
            GameObject newGo = Instantiate(debugObject, vec, Quaternion.identity);
            DebugList.Add(newGo);
        }
    }

    public void ShowPathLine(List<Vector3> path, LineRenderer rendererLine = null)
    {
        LineRenderer lineRenderer;
        if (rendererLine != null) 
        {
            lineRenderer = rendererLine;
        }
        else
        {
            lineRenderer = PathManager.GetInstance.currentLineDebug;
        }

        if(!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
        }

        lineRenderer.positionCount = 0;
        foreach (Vector3 vec in path)
        {
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, vec);
        }
    }
}
