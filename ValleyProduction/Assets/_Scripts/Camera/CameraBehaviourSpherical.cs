using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraBehaviourSpherical : MonoBehaviour
{
    [SerializeField] private Transform cameraTarget = default;
    [SerializeField] private Transform cameraTargetOrigin = default;
    [SerializeField] private Transform originLookAtTarget = default;
    [SerializeField] private CinematicCameraBehaviour cinematicCameraBehaviour = default;
    [SerializeField] private float lerpValue;


    // Update is called once per frame
    void Update()
    {
        JoinTarget();
        JoinTargetNoInterpolation();
        SetCameraForward();

    }

    void JoinTarget()
    {
        transform.position = Vector3.Lerp(transform.position, cameraTarget.position, lerpValue);

    }

    void JoinTargetNoInterpolation()
    {
        if (!cinematicCameraBehaviour)
            return;

        if (!cinematicCameraBehaviour.inCinematicMode)
            return;

        transform.position = cameraTarget.position;
    }

    void SetCameraForward()
    {
        transform.forward = new Vector3(originLookAtTarget.position.x - transform.position.x, originLookAtTarget.position.y - transform.position.y, originLookAtTarget.position.z - transform.position.z);
    }
}
