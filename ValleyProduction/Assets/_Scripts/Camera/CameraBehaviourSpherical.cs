using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraBehaviourSpherical : MonoBehaviour
{
    [SerializeField] private Transform cameraTarget = default;
    [SerializeField] private Transform cameraTargetOrigin = default;
    [SerializeField] private Transform originLookAtTarget = default;
    [SerializeField] private float lerpValue;


    // Update is called once per frame
    void Update()
    {
        JoinTarget();
        SetCameraForward();

    }

    void JoinTarget()
    {
        transform.position = Vector3.Lerp(transform.position, cameraTarget.position, lerpValue);

    }

    void SetCameraForward()
    {
        transform.forward = new Vector3(originLookAtTarget.position.x - transform.position.x, originLookAtTarget.position.y - transform.position.y, originLookAtTarget.position.z - transform.position.z);
    }
}
