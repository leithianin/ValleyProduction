using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class CameraManager : VLY_Singleton<CameraManager>
{
    [SerializeField] private Camera currentCamera;
    public SphericalTransform spherical;
    public CinematicCameraBehaviour cineCamBehav;
    public PostProcessManager postProcessManager;
    //public GameObject origin;

    public static Action OnCameraMove;
    public static Action OnCameraMoveEnd;

    [SerializeField] private LayerMask cameraLayerMaskBase;

    [SerializeField] private string interactionZoneDisplayMask;

    private void Start()
    {
        cameraLayerMaskBase = currentCamera.cullingMask;
    }

    public static void SetCameraPosition(Vector3 position)
    {
        instance.spherical.SetOrigin(position);
    }

    //Rotation chiant
    public static void MoveCamera(float targetRadius, float targetAzimuthalAngle, float targetPolarAngle, float speed, bool rotate)
    {
        instance.spherical.MoveCameraOverTime(targetRadius, targetAzimuthalAngle, targetPolarAngle, speed);
    }

    public static void SetTarget(Transform tr)
    {
        instance.spherical.SetCameraTarget(tr);
    }

    public static void SetTargetWithSpeed(Transform tr, float speed)
    {
        instance.spherical.StartCoroutine(instance.spherical.MoveCameraOriginToCustomTarget(tr, speed));
    }

    public static void SetVignettage(float value)
    {
        instance.postProcessManager.SetVignetteValue(value);
    }

    public void ChangeInteractionZoneLayerMask(bool showLayer)
    {
        if(showLayer)
        {
            AddCullingLayer(instance.interactionZoneDisplayMask);
        }
        else
        {
            RemoveCullingLayer(instance.interactionZoneDisplayMask);
        }
    }

    public static void AddCullingLayer(string mask)
    {
        instance.cameraLayerMaskBase |= (1 << LayerMask.NameToLayer(mask));
        instance.currentCamera.cullingMask |= (1 << LayerMask.NameToLayer(mask));
    }

    public static void RemoveCullingLayer(string mask)
    {
        instance.cameraLayerMaskBase &= ~(1 << LayerMask.NameToLayer(mask));
        instance.currentCamera.cullingMask &= ~(1 << LayerMask.NameToLayer(mask));
    }

    public static void SetCinematicMode()
    {
        instance.cineCamBehav.inCinematicMode = true;
    }

    public void AddEventLayer(string str)
    {
        currentCamera.eventMask |= (1 << LayerMask.NameToLayer(str));
    }

    public void RemoveEventLayer(string str)
    {
        currentCamera.eventMask &= ~(1 << LayerMask.NameToLayer(str));
    }
}
