using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraBehaviourInEditor : MonoBehaviour
{
    [SerializeField] private Transform cameraTarget = default;


    private void Update()
    {
        if (!Application.isPlaying)
            UpdateCameraPositionInEditor();
    }

    void UpdateCameraPositionInEditor()
    {
        transform.position = cameraTarget.position;
        transform.forward = new Vector3(cameraTarget.position.x - transform.position.x, cameraTarget.position.y - transform.position.y, cameraTarget.position.z - transform.position.z);
        //transform.LookAt(cameraTarget.position);
    }

}
