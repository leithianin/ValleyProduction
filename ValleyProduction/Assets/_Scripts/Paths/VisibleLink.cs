using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VisibleLink : MonoBehaviour
{
    public LineRenderer line;
    private bool isSecondPointIsPlaced = false;
    public int index = 1;
    private Vector3 offsetPathCalcul = new Vector3(0,0,-0.5f);

    private NavMeshPath path;

    private List<Vector3> pathToReturn;

    private void Awake()
    {
        path = new NavMeshPath();
    }

    void Update()
    {
        if (line != null)
        {
            path = new NavMeshPath();
            //if (line.positionCount > 0)
            {
                NavMesh.CalculatePath(Valley_PathManager.GetCurrentMarker.Position + offsetPathCalcul, GetPositionSecondPoint() + offsetPathCalcul, NavMesh.AllAreas, path);

                line.positionCount = index + 1;
                line.SetPosition(index, GetPositionSecondPoint() + offsetPathCalcul);
                List<Vector3> points = new List<Vector3>();

                int j = 1;

                while (j < path.corners.Length)
                {
                    line.positionCount = path.corners.Length;
                    points = new List<Vector3>(path.corners);

                    for (int k = 0; k < points.Count; k++)
                    {

                        line.SetPosition(k, points[k]);
                    }

                    j++;
                }

                if (j == path.corners.Length && path.status == NavMeshPathStatus.PathComplete)
                {
                    pathToReturn = new List<Vector3>(points);
                }

                index = line.positionCount - 1;
            }

            VisibleLinkManager.SetLine(line);
        }
    }

    public void FirstPoint()
    {
        //Remet � 1 l'index si jamais il se retrouve � 0 lors de la suppression
        if(index == 0)
        {
            index = 1;
        }

        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position);               //Sert � �viter qu'il spawn � 0,0,0 le temps d'une frame, le montrant au joueur par la m�me occasion
    }

    private Vector3 GetPositionSecondPoint()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100000.0f))
        {
            return hit.point;
        }

        return Vector3.zero;
    }

    public void AddPoint(GameObject nextObjectToLink, out LineRenderer pathLine)
    {
        line.positionCount++;
        line.SetPosition(line.positionCount-1,  nextObjectToLink.transform.position);

        pathLine = line;

        line = null;
    }

    public void EndPoint(GameObject previousMarker)
    {
        if (line != null)
        {
            Destroy(line.gameObject);
            line = null;
        }
    }

    public void ResetPoint()
    {
        index--;
        line.positionCount--;
    }

    public void SetLine(LineRenderer ln)
    {
        line = ln;
        index = ln.positionCount++;
    }

    public void UpdateLine()
    {
        line = transform.GetChild(1).GetComponent<LineRenderer>();
    }

    public void UpdateLineWithLineKnowed(LineRenderer lineRenderer)
    {
        if(VisibleLinkManager.CurrentLine != null)
        {
            VisibleLinkManager.DestroyLine();
        }
        line = lineRenderer;
        VisibleLinkManager.SetLine(line);
    }

    public LineRenderer FindLineRenderer(PathPoint objectToCheck)
    {
        if (objectToCheck != null)
        {
            for (int i = 1; i < objectToCheck.transform.childCount; i++)
            {
                if (objectToCheck.transform.GetChild(i).GetComponent<LineRenderer>().GetPosition(1) == gameObject.transform.position)
                {
                    return objectToCheck.transform.GetChild(i).GetComponent<LineRenderer>();
                }
            }
        }

        return null;
    }
}
