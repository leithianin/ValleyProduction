using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraBehaviourSpherical : MonoBehaviour
{
    [SerializeField] private Transform cameraTarget = default;
    [SerializeField] private Transform cameraTargetOrigin = default;
    [SerializeField] private float lerpValue;


    // Update is called once per frame
    void Update()
    {
        JoinTarget();
        SetCameraForward();

    }

    private void LateUpdate()
    {
    }

    void JoinTarget()
    {
        transform.position = Vector3.Lerp(transform.position, cameraTarget.position, lerpValue);
        //transform.position = cameraTarget.position;
    }

    void SetCameraForward()
    {
        transform.forward = Vector3.Lerp(transform.forward,cameraTarget.forward, 0.1f);
        //transform.forward = cameraTarget.forward;
    }
}
