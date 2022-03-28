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
    public static List<IST_PathPoint> refList;                                                                     //Copie de la liste de pathpoint de pathManager
    public List<PathFragmentData> newPathFragmentData;
    private IST_PathPoint movingPathpoint;
    private List<PathNode> movingBorderPointsDatas;
    public static List<ModifyListClass> ModifyList = new List<ModifyListClass>();
    private List<AdditionalPathpointClass> additionalPathpointList = new List<AdditionalPathpointClass>();
    public static bool isModifyPath = false;
    private float distance = 0f;

    [Header("PathRenderer")]
    public PathRendererManager pathRendererManager;

    public IST_PathPoint testPathpoint;

    public static IST_PathPoint MovingPathPoint => instance.movingPathpoint;
    public static List<AdditionalPathpointClass> GetAdditionnalPoints => instance.additionalPathpointList;
    public static List<PathNode> GetMovingBorderPoints => instance.movingBorderPointsDatas;

    //List des pathFragment à modifier avec leur LineRenderer
    public class ModifyListClass
    {
        public LineRenderer modifyLinesRenderer;
        //public List<LineRenderer> 
        public PathFragmentData modifyPathFragment;
        public bool inverse;

        public ModifyListClass(LineRenderer modifyLinesRendererV, PathFragmentData modifyPathFragmentV, bool isInverse)
        {
            modifyLinesRenderer = modifyLinesRendererV;
            modifyPathFragment = modifyPathFragmentV;
            inverse = isInverse;
        }
    }

    public class AdditionalPathpointClass
    {
        public List<IST_PathPoint> pathpointList = new List<IST_PathPoint>();
        public bool isBefore = false;
    }

    public static void GetUpdateSeveralLine(IST_PathPoint pp) => instance.UpdateSeveralLines(pp);

    private void Awake()
    {
        instance = this;
        navPath = new NavMeshPath();
    }

    // Update is called once per frame
    void Update()
    {
        if (PathManager.GetInstance.currentLineDebug)
        {
            CalculateCurrentPath();
        }

        if (isModifyPath)
        {
            UpdateSeveralLines(movingPathpoint);
        }
    }

    public static void ResetData()
    {
        isModifyPath = false;
        instance.newPathFragmentData.Clear();
        instance.additionalPathpointList.Clear();
        ModifyList.Clear();
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

    //Calculate Path between 2 points (Use when i recreate a path and i know all the points)
    public List<Vector3> CalculatePath(IST_PathPoint ppstart, IST_PathPoint ppend)
    {
        navPath = new NavMeshPath();

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

        return navmeshPositionsList;
    }

    //Update les line renderer lorsqu'on déplace un pathpoint
    public void UpdateSeveralLines(IST_PathPoint pp)
    {
        movingPathpoint = pp;

        movingBorderPointsDatas = movingPathpoint.Node.GetNeighbours();

        while (ModifyList.Count > additionalPathpointList.Count)
        {
            additionalPathpointList.Add(new AdditionalPathpointClass());
        }

        for (int i = 0; i < ModifyList.Count; i++)
        {
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

            AddMarkers(navmeshPositionsList, i, ModifyList[i].inverse);

            //DebugNavmesh();

            ShowPathLine(navmeshPositionsList, ModifyList[i].modifyLinesRenderer);
        }
    }

    public static bool IsPathShortEnough(float maxDistance)
    {
        float currentDistance = 0;

        for (int i = 0; i < navmeshPositionsList.Count - 1; i++)
        {
            currentDistance += Vector3.Distance(navmeshPositionsList[i], navmeshPositionsList[i + 1]);

            if (currentDistance > maxDistance)
            {
                return false;
            }
        }
        return true;
    }

    //Add markers beetween the moving pathpoint and other pathpoints if the distance is to high /!\ Possibilité de rassembler des choses en une fonction /!\
    public void AddMarkers(List<Vector3> vectors, int index, bool isInverse)
    {
        float distance = Vector3.Distance(vectors[0], vectors[vectors.Count - 1]);
        int result = (int)distance / 20;                                                                            //Replace 20 par la valeur de distanceMax entre les balises

        if (result > 0)
        {
            for (int i = 1; i <= result; i++)
            {
                if (additionalPathpointList[index].pathpointList.Count < result)
                {
                    additionalPathpointList[index].pathpointList.Add(new IST_PathPoint());                                         //J'ajoute un Pathpoint Null à l'index pour mieux le modifer après
                }

                float distancemax = Vector3.Distance(vectors[0], vectors[vectors.Count - 1]);

                //La distance qu'il faut parcourir à partir du point qu'on déplace
                float distanceNeed = distancemax - (i * 20f);

                if (!isInverse)                                                                                                     //Si chemin est dans le bon sens
                {
                    additionalPathpointList[index].isBefore = true;

                    for (int y = 0; y < vectors.Count - 1; y++)
                    {
                        float distanceToSave = Vector3.Distance(vectors[y], vectors[y + 1]);

                        if (distanceNeed < distanceToSave)
                        {
                            if (additionalPathpointList[index].pathpointList[i - 1] == null)
                            {
                                //Placer le point
                                Vector3 positionMarker = ValleyUtilities.GetVectorPoint3D(vectors[y], vectors[y + 1], (Mathf.Abs(distanceNeed) / Vector3.Distance(vectors[y], vectors[y + 1])));
                                IST_PathPoint pathpoint = Instantiate(testPathpoint, positionMarker, Quaternion.identity);                                         //Remplacer par le pathpoint Preview
                                UpdateData(pathpoint, vectors, false);
                                additionalPathpointList[index].pathpointList[i - 1] = pathpoint;

                            }
                            else
                            {
                                //Update point
                                additionalPathpointList[index].pathpointList[i - 1].transform.position = ValleyUtilities.GetVectorPoint3D(vectors[y], vectors[y + 1], (Mathf.Abs(distanceNeed) / Vector3.Distance(vectors[y], vectors[y + 1])));
                                break;
                            }
                        }
                        else
                        {
                            distanceNeed -= Vector3.Distance(vectors[y], vectors[y + 1]);
                        }
                    }
                }
                else                                                                                                               //Si chemin est dans le sens inverse
                {
                    for (int y = vectors.Count - 1; y > 0; y--)
                    {
                        float distanceToSave = Vector3.Distance(vectors[y], vectors[y - 1]);

                        if (distanceNeed < distanceToSave)
                        {
                            if (additionalPathpointList[index].pathpointList[i - 1] == null)
                            {
                                //Placer le point
                                Vector3 positionMarker = ValleyUtilities.GetVectorPoint3D(vectors[y], vectors[y - 1], (Mathf.Abs(distanceNeed) / Vector3.Distance(vectors[y], vectors[y - 1])));
                                IST_PathPoint pathpoint = Instantiate(testPathpoint, positionMarker, Quaternion.identity);                                        //Remplacer par le pathpoint Preview
                                UpdateData(pathpoint, vectors, true);
                                additionalPathpointList[index].pathpointList[i - 1] = pathpoint;
                            }
                            else
                            {
                                additionalPathpointList[index].pathpointList[i - 1].transform.position = ValleyUtilities.GetVectorPoint3D(vectors[y], vectors[y - 1], (Mathf.Abs(distanceNeed) / Vector3.Distance(vectors[y], vectors[y - 1])));
                                break;
                            }
                        }
                        else
                        {
                            distanceNeed -= Vector3.Distance(vectors[y], vectors[y - 1]);
                        }
                    }
                }
            }
        }

        //Retire les points en trop si la distance à diminué
        if (result < additionalPathpointList[index].pathpointList.Count)
        {
            for (int i = additionalPathpointList[index].pathpointList.Count; additionalPathpointList[index].pathpointList.Count > result; i--)
            {
                PathManager.instance.pathpointList.Remove(additionalPathpointList[index].pathpointList[additionalPathpointList[index].pathpointList.Count - 1]);
                Destroy(additionalPathpointList[index].pathpointList[additionalPathpointList[index].pathpointList.Count - 1].gameObject);
                additionalPathpointList[index].pathpointList.RemoveAt(additionalPathpointList[index].pathpointList.Count - 1);
                //Delete PathPoint dans la liste
            }
        }
    }

    //Update les info du pathdata de PathManager 
    //PathFragment à update ici aussi
    public void UpdateData(IST_PathPoint pp, List<Vector3> vectors, bool isBefore)
    {
        int index = refList.IndexOf(movingPathpoint);
        refList.Add(pp);

        //Je pars du dernier point et je reviens vers l'index de celui que je bouge
        for (int i = refList.Count-1; i >= index; i--)
        {
            refList[i] = refList[i - 1];

            if (isBefore)
            {
                if(i == index)
                {
                    refList[i] = pp;
                    //update pathf

                    break;
                }
            }
            else
            {
                if(i == (index+1))
                {
                    refList[i] = pp;
                    //update  athf

                    break;
                }
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
            GameObject newGo = Instantiate(debugObject, vec, Quaternion.identity);
            DebugList.Add(newGo);
        }
    }

    //Montre le LineRenderer selon le path qu'on lui donne
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
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, new Vector3(vec.x, vec.y + 0.25f, vec.z));
        }
    }
}
