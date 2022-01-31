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


    private List<RaycastHit> raycastHitList = new List<RaycastHit>();
    RaycastHit hitBeneath;
    RaycastHit hitFront;
    RaycastHit hitRight;
    RaycastHit hitBack;
    RaycastHit hitLeft;
    private List<float> raycastsLengths = new List<float>();
    float hitBeneathDistance = default;
    float hitFrontDistance = default;
    float hitRightDistance = default;
    float hitBackDistance = default;
    float hitLeftDistance = default;

    float minOffsetLength = default;



    private Vector3 offset = default;

    private void Awake()
    {
        AddRaycastsLengthsToList();
        minOffsetLength = 5;
        Debug.Log(minOffsetLength);
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();
        FixXRotation();
        CheckDistancesToGround();
        SetOffset();
    }

    void AddRaycastsLengthsToList()
    {
        raycastsLengths.Add(hitBeneathDistance);
        raycastsLengths.Add(hitFrontDistance);
        raycastsLengths.Add(hitRightDistance);
        raycastsLengths.Add(hitBackDistance);
        raycastsLengths.Add(hitLeftDistance);
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
        //if ( hitBeneath.distance <= minDistToEnviro)
        //{
        //    offset = new Vector3(0f, minDistToEnviro - hitBeneath.distance, 0f);
        //}
        //else
        //{
        //    offset = Vector3.zero;
        //    offsetIncrement = 0;
        //}

        for (int i = 0; i < raycastsLengths.Count; i++)
        {
            //Debug.Log(minOffsetLength);
            if (raycastsLengths[i] < minOffsetLength)
            {
                //Debug.Log(raycastsLengths[i]);
                minOffsetLength = raycastsLengths[i];

            }
        }

        if (minOffsetLength <= minDistToEnviro)
        {
            //Debug.Log(minDistToEnviro);
            offset = GetOffsetVector(minOffsetLength);
        }
        else
        {
            offset = Vector3.zero;
            minOffsetLength = minDistToEnviro;
        }

    }

    Vector3 GetOffsetVector(float offsetLength)
    {
        Vector3 beneathOffset = hitBeneath.distance <= minDistToEnviro ? Vector3.up : Vector3.zero;
        Vector3 frontOffset = hitFront.distance <= minDistToEnviro ? Vector3.up + Vector3.back : Vector3.zero;
        Vector3 rightOffset = hitRight.distance <= minDistToEnviro ? Vector3.up + Vector3.left : Vector3.zero;
        Vector3 backOffset = hitBack.distance <= minDistToEnviro ? Vector3.up + Vector3.forward : Vector3.zero;
        Vector3 leftOffset = hitLeft.distance <= minDistToEnviro ? Vector3.up + Vector3.right : Vector3.zero;

        Vector3 offset = beneathOffset + frontOffset + rightOffset + backOffset + leftOffset;
        offset.Normalize();
        offset = offset * (minDistToEnviro - offsetLength);
        return offset;
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

    void CheckDistancesToGround()
    {
        //Down
        if (Physics.Raycast(playerCamera.position, cameraPosTarget.TransformDirection(Vector3.Normalize(Vector3.down)), out hitBeneath, Mathf.Infinity))
        {
            Debug.DrawRay(playerCamera.position, cameraPosTarget.TransformDirection(Vector3.Normalize(Vector3.down)) * hitBeneath.distance, Color.yellow);
            raycastsLengths[0] = hitBeneath.distance;
        }
        //Down Front
        if (Physics.Raycast(playerCamera.position, cameraPosTarget.TransformDirection(Vector3.Normalize(Vector3.down + Vector3.forward)), out hitFront, Mathf.Infinity))
        {
            Debug.DrawRay(playerCamera.position, cameraPosTarget.TransformDirection(Vector3.Normalize(Vector3.down + Vector3.forward)) * hitFront.distance, Color.yellow);
            raycastsLengths[1] = hitFront.distance;
        }
        //Down Right
        if (Physics.Raycast(playerCamera.position, cameraPosTarget.TransformDirection(Vector3.Normalize(Vector3.down + Vector3.right)), out hitRight, Mathf.Infinity))
        {
            Debug.DrawRay(playerCamera.position, cameraPosTarget.TransformDirection(Vector3.Normalize(Vector3.down + Vector3.right)) * hitRight.distance, Color.yellow);
            raycastsLengths[2] = hitRight.distance;
        }
        //Down Back
        if (Physics.Raycast(playerCamera.position, cameraPosTarget.TransformDirection(Vector3.Normalize(Vector3.down + Vector3.back)), out hitBack, Mathf.Infinity))
        {
            Debug.DrawRay(playerCamera.position, cameraPosTarget.TransformDirection(Vector3.Normalize(Vector3.down + Vector3.back)) * hitBack.distance, Color.yellow);
            raycastsLengths[3] = hitBack.distance;

        }
        //Down Left
        if (Physics.Raycast(playerCamera.position, cameraPosTarget.TransformDirection(Vector3.Normalize(Vector3.down + Vector3.left)), out hitLeft, Mathf.Infinity))
        {
            Debug.DrawRay(playerCamera.position, cameraPosTarget.TransformDirection(Vector3.Normalize(Vector3.down + Vector3.left)) * hitLeft.distance, Color.yellow);
            raycastsLengths[4] = hitLeft.distance;

        }

    }

    void CheckFrontDistance()
    {
        if (Physics.Raycast(cameraPosTarget.position, cameraPosTarget.TransformDirection(Vector3.forward), out hitFront, Mathf.Infinity))
        {
            Debug.DrawRay(cameraPosTarget.position, cameraPosTarget.TransformDirection(Vector3.forward) * hitFront.distance, Color.yellow);
        }
    }
}
