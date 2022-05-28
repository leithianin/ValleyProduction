using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineLastMarker : MonoBehaviour
{
    private IST_PathPoint currentPathpoint;

    private void Update()
    {
        Outline();
    }

    private void OnDisable()
    {
        DesactivateOutline();
    }

    public void ActivateOutline()
    {
        currentPathpoint = PathManager.GetCurrentPathpointList[PathManager.GetCurrentPathpointList.Count - 1];
        Outline();
    }

    public void Outline()
    {
        if (PathManager.GetCurrentPathpointList.Count > 0)
        {
            PathManager.GetCurrentPathpointList[PathManager.GetCurrentPathpointList.Count - 1].outline.enabled = true;
        }
    }

    public void DesactivateOutline()
    {
        PathManager.GetCurrentPathpointList[PathManager.GetCurrentPathpointList.Count - 1].outline.enabled = false;
        enabled = false;
    }
}
