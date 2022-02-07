using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [HideInInspector] public float minDistToEnviro = default;
    [HideInInspector] public float rotationSpeed = default;

    [SerializeField] private Transform cameraViewTarget = default;
    [SerializeField] private Transform cameraPosTarget = default;
    [SerializeField] private Transform playerCamera = default;

    float minOffsetLength = default;

    private Vector3 offset = default;

    public Vector3 direction;

    public Vector3 Direction { get; set; }

    private void Awake()
    {
        minOffsetLength = 5;
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();
        FixXRotation();
        SetOffset();
        CheckDistanceToGround();
    }


    void MoveCamera()
    {
        //transform.position = Vector3.Lerp(transform.position, cameraPosTarget.position + offset, 0.05f);
        transform.position = Vector3.Lerp(transform.position, new Vector3(cameraPosTarget.position.x, Mathf.Clamp(cameraPosTarget.position.y, Terrain.activeTerrain.SampleHeight(transform.position) + minDistToEnviro, 1000f), cameraPosTarget.position.z), 0.05f);
        Debug.Log(Terrain.activeTerrain.SampleHeight(transform.position) + minDistToEnviro);
    }

    void FixXRotation()
    {
        Vector3 forward = new Vector3(cameraViewTarget.position.x - transform.position.x, cameraViewTarget.position.y - transform.position.y + offset.y, cameraViewTarget.position.z - transform.position.z);
        playerCamera.forward = Vector3.Lerp(playerCamera.forward, forward, 0.01f);
    }

    void SetOffset()
    {
        if (raycastHitTest.distance <= minDistToEnviro)
        {
            offset = new Vector3(0f, (Terrain.activeTerrain.SampleHeight(transform.position) - cameraPosTarget.position.y) < 0 ? (Terrain.activeTerrain.SampleHeight(transform.position) - cameraPosTarget.position.y) : 0.0f + (minDistToEnviro - raycastHitTest.distance), 0f);
            Debug.Log((Terrain.activeTerrain.SampleHeight(transform.position) - cameraPosTarget.position.y) < 0 ? (Terrain.activeTerrain.SampleHeight(transform.position) - cameraPosTarget.position.y) : 0.0f + (minDistToEnviro - raycastHitTest.distance));
        }
        //else if (raycastHitTest.distance > 5)
        //{
        //    offset = Vector3.zero;
        //}
    }

    RaycastHit raycastHitTest;
    void CheckDistanceToGround()
    {
        //Debug.Log(direction);

        if (Physics.Raycast(playerCamera.position, cameraPosTarget.TransformDirection(Vector3.Normalize(Vector3.down + direction)), out raycastHitTest, Mathf.Infinity))
        {
            Debug.DrawRay(playerCamera.position, cameraPosTarget.TransformDirection(Vector3.Normalize(Vector3.down + direction)) * raycastHitTest.distance, Color.blue);
        }

        
    }
}
