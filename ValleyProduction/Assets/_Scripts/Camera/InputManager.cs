using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private SphericalTransform cameraTransform = default;
    [SerializeField] private Transform cameraOrigin = default;

    [SerializeField] private bool edgeScrolling;

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
        EdgeScrolling();
        MoveOrigin();
        SetDistanceToOrigin();
        RotateCamera();
        RotateWithScrollWheel();
        //MouseCameraMovement();
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

    void EdgeScrolling()
    {
        if (!edgeScrolling)
            return;

        if (Input.mousePosition.x > 0 && Input.mousePosition.x < Screen.width && Input.mousePosition.y > 0 && Input.mousePosition.y < Screen.height) //Check if the mouse is on the borders of the screen
            return;

        Vector2 mouseDirection = new Vector2(Input.mousePosition.x - (Screen.width / 2), Input.mousePosition.y - (Screen.height / 2)); //Convert Mouse position into direction vector for moving origin
        mouseDirection.Normalize();
        cameraTransform.MoveOrigin(mouseDirection.x, mouseDirection.y, movingSpeed);
    }

    void MouseCameraMovement()
    {
        if (!Input.GetKey(KeyCode.Mouse1))
            return;

        Vector3 mouseOriginPosition = Vector3.zero;
        Vector3 originPosition;

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            mouseOriginPosition = Input.mousePosition;
            cameraTransform.OiriginStartPosition = cameraTransform.Origin.position;
        }


        Vector2 movingVector = Input.mousePosition - mouseOriginPosition;


    }
}
