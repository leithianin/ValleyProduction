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
        transform.forward = cameraTarget.forward;
    }

}
