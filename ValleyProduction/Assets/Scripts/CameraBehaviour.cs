using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public float minDistToEnviro = default;
    public float rotationSpeed = default;

    [SerializeField] private Transform cameraViewTarget = default;
    [SerializeField] private Transform cameraPosTarget = default;
    [SerializeField] private Transform playerCamera = default;

    private Vector3 offset = default;

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
        transform.position = Vector3.Lerp(transform.position, cameraPosTarget.position  + offset, 0.05f);
    }

    void FixXRotation()
    {
        Vector3 forward = new Vector3(cameraViewTarget.position.x - transform.position.x, cameraViewTarget.position.y - transform.position.y + offset.y, cameraViewTarget.position.z - transform.position.z);
        playerCamera.forward = Vector3.Lerp(playerCamera.forward, forward, 0.01f);
    }

    void SetOffset()
    {
        if ( hitBeneath.distance <= minDistToEnviro)
        {
            offset = new Vector3(0f, minDistToEnviro - hitBeneath.distance, 0f);
        }
        else
        {
            offset = Vector3.zero;
            offsetIncrement = 0;
        }
    }

    float offsetIncrement;
    void OffsetIncrement()
    {
        if (hitFront.distance <= 3)
        {
            offsetIncrement = offsetIncrement + 0.05f;
            Debug.Log(hitBeneath.distance);
        }
    }

    RaycastHit hitBeneath;
    void CheckDistanceToGround()
    {
        if (Physics.Raycast(cameraPosTarget.position, cameraPosTarget.TransformDirection(Vector3.down), out hitBeneath, Mathf.Infinity))
        {
            Debug.DrawRay(cameraPosTarget.position, cameraPosTarget.TransformDirection(Vector3.down) * hitBeneath.distance, Color.yellow);
        }
    }

    RaycastHit hitFront;
    void CheckFrontDistance()
    {
        if (Physics.Raycast(cameraPosTarget.position, cameraPosTarget.TransformDirection(Vector3.forward), out hitFront, Mathf.Infinity))
        {
            Debug.DrawRay(cameraPosTarget.position, cameraPosTarget.TransformDirection(Vector3.forward) * hitFront.distance, Color.yellow);
        }
    }
}
