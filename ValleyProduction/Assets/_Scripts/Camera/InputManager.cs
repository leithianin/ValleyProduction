using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private SphericalTransform cameraTransform = default;

    [SerializeField] private float movingSpeed = 10.0f;
    [SerializeField, Tooltip("Degrees per second")] private float rotationSpeed = 90.0f;
    [SerializeField, Tooltip("When Scrolling Wheel is pressed")] private float wheelRotationSpeed = 90.0f;

    [SerializeField] private float scrollingSpeed = 100f;

    private void OnEnable()
    {
        if (!cameraTransform)
            Debug.LogError("Cannot find Camera Transform");
    }

    void Update()
    {
        MoveOrigin();
        SetDistanceToOrigin();
        RotateCamera();
        RotateWithScrollWheel();
    }

    void MoveOrigin()
    {
        if (!cameraTransform)
            return;

        cameraTransform.MoveOrigin(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), movingSpeed);
    }

    void RotateCamera()
    {
        if (!cameraTransform)
            return;

        cameraTransform.AzimuthalRotation(Input.GetAxis("Azimuthal"), rotationSpeed);
        cameraTransform.PolarRotation(Input.GetAxis("Polar"), rotationSpeed);
    }

    void SetDistanceToOrigin()
    {
        if (!cameraTransform)
            return;

        cameraTransform.ChangeLength(Input.GetAxis("Mouse ScrollWheel"), -scrollingSpeed);
    }

    void RotateWithScrollWheel()
    {
        if (!cameraTransform)
            return;

        if (!Input.GetKey(KeyCode.Mouse2))
            return;

        cameraTransform.AzimuthalRotation(Input.GetAxis("Mouse X"), wheelRotationSpeed);
        cameraTransform.PolarRotation(Input.GetAxis("Mouse Y"), wheelRotationSpeed);
    }
}
