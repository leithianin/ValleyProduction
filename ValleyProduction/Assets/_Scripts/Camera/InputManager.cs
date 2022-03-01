using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private SphericalTransform cameraTransform = default;
    [SerializeField] private CinematicCameraBehaviour cinematicCameraBehaviour = default;
    [SerializeField] private Transform cameraOrigin = default;
    [SerializeField] private GameObject hud = default;

    [SerializeField] private float movingSpeed = 10.0f;

    [Header("Edge Scrolling")]
    [SerializeField] private bool useEdgeScrolling;
    [SerializeField, Range(1,20)] private float edgeScrollingMovingSpeed = 10f;

    [Header("Mouse Scrolling")]
    [SerializeField] private bool useMouseScrolling;
    [SerializeField, Range(1, 20)] private float mouseScrollingMovingSpeed = 10f;


    [Header("Mouse Wheel Values")]
    [SerializeField, Tooltip("Degrees per second")] private float rotationSpeed = 90.0f;
    [SerializeField, Tooltip("When Scrolling Wheel is pressed")] private float wheelRotationSpeed = 90.0f;

    [SerializeField] private float scrollingSpeed = 100f;

    [Header("Cinematic Mode")]
    [SerializeField, ReadOnly] private bool inCinematicMode = false;

    private void OnEnable()
    {
        if (!cameraTransform)
            Debug.LogError("Cannot find Camera Transform");
    }


    void Update()
    {
        //Move Camera functions
        MoveCameraOriginWithEdgeScrolling();
        MoveCameraOriginWithKeyboard();
        MoveCameraOriginWithMouseDrag();

        //Zoom in-out function
        SetDistanceToOrigin();

        //Rotate Camera functions
        RotateCameraWithKeyboard();
        RotateCameraWithScrollWheel();

        //Handle Cinematic Mode
        StopCinematicMode();
        LaunchCinematicMode();
    }

    void MoveCameraOriginWithKeyboard()
    {
        if (!cameraTransform)
            return;

        cameraTransform.MoveOrigin(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), movingSpeed);
    }

    void MoveCameraOriginWithEdgeScrolling()
    {
        if (!useEdgeScrolling)
            return;

        if (Input.GetKey(KeyCode.Mouse1))
            return;

        if (Input.mousePosition.x > 0 && Input.mousePosition.x < Screen.width && Input.mousePosition.y > 0 && Input.mousePosition.y < Screen.height) //Check if the mouse is on the borders of the screen
            return;

        Vector2 mouseDirection = new Vector2(Input.mousePosition.x - (Screen.width / 2), Input.mousePosition.y - (Screen.height / 2)); //Convert Mouse position into direction vector for moving origin
        mouseDirection.Normalize();
        cameraTransform.MoveOrigin(mouseDirection.x, mouseDirection.y, edgeScrollingMovingSpeed);
    }

    void MoveCameraOriginWithMouseDrag()
    {
        if (!useMouseScrolling)
            return;

        if (!Input.GetKey(KeyCode.Mouse1))
            return;

        Vector2 mouseDirection = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        mouseDirection = -mouseDirection;
        cameraTransform.MoveOrigin(mouseDirection.x, mouseDirection.y, mouseDirection.magnitude * mouseScrollingMovingSpeed);
    }


    void SetDistanceToOrigin()
    {
        if (!cameraTransform)
            return;

        cameraTransform.ChangeLength(Input.GetAxis("Mouse ScrollWheel"), -scrollingSpeed);
    }

    void RotateCameraWithKeyboard()
    {
        if (!cameraTransform)
            return;

        cameraTransform.AzimuthalRotation(Input.GetAxis("Azimuthal"), rotationSpeed);
        cameraTransform.PolarRotation(Input.GetAxis("Polar"), rotationSpeed);
    }

    void RotateCameraWithScrollWheel()
    {
        if (!cameraTransform)
            return;

        if (!Input.GetKey(KeyCode.Mouse2))
            return;

        cameraTransform.AzimuthalRotation(Input.GetAxis("Mouse X"), wheelRotationSpeed);
        cameraTransform.PolarRotation(Input.GetAxis("Mouse Y"), wheelRotationSpeed);
    }

    void LaunchCinematicMode()
    {
        if (!Input.GetKeyDown(KeyCode.C))
            return;

        if (cinematicCameraBehaviour.inCinematicMode)
            return;

        cinematicCameraBehaviour.cinematicModeTriggered = true;
        hud.SetActive(false);
    }

    void StopCinematicMode()
    {
        if (!cinematicCameraBehaviour.cinematicModeTriggered)
            return;

        if (Input.anyKeyDown)
        {
            cinematicCameraBehaviour.cinematicModeTriggered = false;
            cinematicCameraBehaviour.inCinematicMode = false;
            cinematicCameraBehaviour.FadeReset();
            cinematicCameraBehaviour.StopAllCoroutines();
            hud.SetActive(true);
        }
    }
}
