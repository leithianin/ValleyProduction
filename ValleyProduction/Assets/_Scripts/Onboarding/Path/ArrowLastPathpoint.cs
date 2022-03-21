using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowLastPathpoint : MonoBehaviour
{
    public GameObject OB_ArrowLastPathpoint;

    public void Update()
    {
        if (OB_ArrowLastPathpoint.activeSelf)
        {
            UpdatePosition();
        }
    }

    public void ActivateArrowLastPathpoint()
    {
        UpdatePosition();
        OB_ArrowLastPathpoint.SetActive(true);
    }

    public void UpdatePosition()
    {
        if (PathManager.GetCurrentPathpointList.Count > 0)
        {
            Vector3 positionLastPathPoint = PathManager.GetCurrentPathpointList[PathManager.GetCurrentPathpointList.Count - 1].transform.position;

            Vector2 canvasPos;
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(positionLastPathPoint);

            OB_ArrowLastPathpoint.transform.position = new Vector3(screenPoint.x, screenPoint.y + 10f);
        }
    }
}
