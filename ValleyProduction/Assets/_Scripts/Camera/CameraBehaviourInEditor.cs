using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraBehaviourInEditor : MonoBehaviour
{
    [SerializeField] private Transform cameraTarget = default;
    [SerializeField] private Transform cameraLookatTarget = default;
    [SerializeField] private SphericalTransform sphericalTransform = default;


    private void Update()
    {
        if (!Application.isPlaying)
            UpdateCameraPositionInEditor();
    }

    void UpdateCameraPositionInEditor()
    {
        transform.position = cameraTarget.position;
        //transform.forward = new Vector3(cameraTarget.position.x - transform.position.x, cameraTarget.position.y - transform.position.y, cameraTarget.position.z - transform.position.z);
        transform.LookAt(cameraLookatTarget.position + new Vector3(0.0f, sphericalTransform.OriginVisualOffset, 0.0f));
    }

}
