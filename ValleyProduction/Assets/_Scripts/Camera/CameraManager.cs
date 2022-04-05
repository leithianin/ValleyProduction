using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : VLY_Singleton<CameraManager>
{
    public SphericalTransform spherical;
    public GameObject origin;


    public static void MoveCamera(float targetRadius, float targetAzimuthalAngle, float targetPolarAngle, float speed)
    {
        instance.spherical.MoveCameraOverTime(targetPolarAngle, targetAzimuthalAngle, targetPolarAngle, speed);
    }

    public static void UpdatePositionOrigin(Vector3 position)
    {
        instance.origin.transform.position = position;
    }
}
