using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineLastMarker : MonoBehaviour
{
    private void Update()
    {
        ActivateOutline();
    }

    private void OnDisable()
    {
        DesactivateOutline();
    }

    public void ActivateOutline()
    {
        PathManager.GetCurrentPathpointList[PathManager.GetCurrentPathpointList.Count - 1].outline.enabled = true;
    }

    public void DesactivateOutline()
    {
        PathManager.GetCurrentPathpointList[PathManager.GetCurrentPathpointList.Count - 1].outline.enabled = false;
    }
}
