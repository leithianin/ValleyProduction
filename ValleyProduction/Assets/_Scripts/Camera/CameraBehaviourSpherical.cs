using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviourSpherical : MonoBehaviour
{
    [SerializeField] private Transform cameraTarget = default;
    [SerializeField] private Transform cameraTargetOrigin = default;
    [SerializeField] private float lerpValue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

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
    }

    void SetCameraForward()
    {
        transform.forward = Vector3.Lerp(transform.forward,cameraTarget.forward, 0.1f);
    }
}
