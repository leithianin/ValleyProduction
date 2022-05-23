using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakePathpointPosition : MonoBehaviour
{
    public IST_PathPoint pathpoint1;
    public Transform pos1;
    public IST_PathPoint pathpoint2;
    public Transform pos2;

    void Start()
    {

    }

    [ContextMenu("CreatePath")]
    public void CreatePath()
    {
        pathpoint1.transform.position = pos1.position;
        pathpoint2.transform.position = pos2.position;
        pathpoint1.PlaceObject(pos1.position);
        pathpoint2.PlaceObject(pos2.position);

        PathManager.CreatePathData();
        PathData pd = PathManager.GetPathData(pathpoint1);
        pd.GetPathFragments(pathpoint1)[0].path = PathCreationManager.GetCalculatePath(pathpoint1, pathpoint2);
    }
}
