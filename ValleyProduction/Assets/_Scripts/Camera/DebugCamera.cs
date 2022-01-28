using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DebugCamera : MonoBehaviour
{
    [SerializeField] private Transform cameraTarget = default;
    [SerializeField] private Transform cameraAnchorXZ = default;
    [SerializeField] private Transform cameraAnchorY = default;

    // Update is called once per frame
    void Update()
    {
        DrawDebug();
    }

    void DrawDebug()
    {
        Debug.DrawLine(cameraAnchorY.position, cameraAnchorXZ.position, Color.green);
        Debug.DrawLine(cameraTarget.position, cameraAnchorXZ.position, Color.green);
        Debug.DrawRay(cameraTarget.position, cameraTarget.forward, Color.red);
    }
}
