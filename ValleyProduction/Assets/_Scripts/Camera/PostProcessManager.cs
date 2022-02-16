using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessManager : MonoBehaviour
{
    [SerializeField] private SphericalTransform cameraSphericalTransform = default;

    [Header("PostProcess Referecences")]
    [SerializeField] private Volume volume = default;
    VolumeProfile profile;

    [Header("Depth of Field Values")]
    [SerializeField, Tooltip("Only apply on Awake")] private bool _useDepthOfField;
    [SerializeField] private float activationDOFDistance;
    [SerializeField] private float standartDOF;
    [SerializeField] private float closeViewDOF;
    private DepthOfField dof = default;

    private void Awake()
    {
        GetPostProcessDOF();
        SetDOFState();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeDOF();
    }

    void GetPostProcessDOF()
    {
        if (!volume)
        {
            Debug.LogWarning("Cannot find sharedProfile");
            return;
        }

        if (!volume.sharedProfile)
        {
            Debug.LogWarning("Cannot find sharedProfile");
            return;
        }

        volume.profile.TryGet<DepthOfField>(out dof);
        standartDOF = dof.gaussianEnd.value;
        
    }

    void SetDOFState()
    {
        dof.active = _useDepthOfField;
    }

    void ChangeDOF()
    {
        if (!dof.active)
            return;

        if (cameraSphericalTransform.GetTargetDistanceToOrigin() < activationDOFDistance)
        {
            dof.gaussianEnd.value = closeViewDOF;
        }
        else
        {
            dof.gaussianEnd.value = standartDOF;
        }
    }
}
