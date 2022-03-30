using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class CameraInputManager : MonoBehaviour
{
    [SerializeField] private SphericalTransform cameraTransform = default;
    [SerializeField] private CinematicCameraBehaviour cinematicCameraBehaviour = default;
    [SerializeField] private GameObject hud = default;
    [SerializeField] private bool allowRotation = true;

    [SerializeField] private float movingSpeed = 5.0f;
    [SerializeField] private float fastMovingSpeed = 10.0f;

    [SerializeField] private SettingsDatas settingsDatas;

    [Header("Edge Scrolling")]
    [SerializeField, Range(1,20)] private float edgeScrollingMovingSpeed = 10f;

    [Header("Mouse Scrolling")]
    [SerializeField, Range(1, 20)] private float mouseScrollingMovingSpeed = 10f;


    [Header("Mouse Wheel Values")]
    [SerializeField, Tooltip("Degrees per second")] private float rotationSpeed = 90.0f;
    [SerializeField, Tooltip("When Scrolling Wheel is pressed")] private float wheelRotationSpeed = 90.0f;

    [SerializeField] private float scrollingSpeed = 100f;

    [Header("Cinematic Mode")]
    [SerializeField, ReadOnly] private bool inCinematicMode = false;

    //Input datas
     private Vector2 inputDirection;
     private Vector2 mouseDirection;
     private bool isShifting;
     private float polarValue;
     private float azimuthalValue;
    private float scrollInputValue;


    public void SetInputDirection(Vector2 nInputDirection)
    {
        Debug.Log(nInputDirection);
        inputDirection = nInputDirection;
    }

    public void SetMouseDirection(Vector2 nMouseDirection)
    {
        mouseDirection = nMouseDirection;
    }

    public void SetShifting(bool nShifting)
    {
        isShifting = nShifting;
    }

    public void SetPolarValue(float nPolarValue)
    {
        polarValue = nPolarValue;
    }

    public void SetAzimythalValue(float nAzimuthalValue)
    {
        azimuthalValue = nAzimuthalValue;
    }

    public void SetScrollInputValue(float nScroll)
    {
        scrollInputValue = nScroll;
    }

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
        if (!cameraTransform || inputDirection == Vector2.zero)
            return;

        cameraTransform.MoveOrigin(inputDirection.x, inputDirection.y, isShifting ? fastMovingSpeed :  movingSpeed);
    }

    void MoveCameraOriginWithEdgeScrolling()
    {
        if (!settingsDatas.cameraEdgeScrollingActive)
            return;

        if (cinematicCameraBehaviour.inCinematicMode)
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
        if (!settingsDatas.cameraMouseScrollingActive || mouseDirection == Vector2.zero)
        {
            return;
        }

        if (!Input.GetKey(KeyCode.Mouse1))
            return;

        mouseDirection = -mouseDirection;
        cameraTransform.MoveOrigin(mouseDirection.x, mouseDirection.y, mouseDirection.magnitude * mouseScrollingMovingSpeed);
    }


    void SetDistanceToOrigin()
    {
        if (!cameraTransform)
            return;

        cameraTransform.ChangeLength(scrollInputValue, -scrollingSpeed);
    }

    void RotateCameraWithKeyboard()
    {
        if (!cameraTransform)
            return;

        cameraTransform.PolarRotation(polarValue, rotationSpeed);

        if (!allowRotation)
            return;

        cameraTransform.AzimuthalRotation(azimuthalValue, rotationSpeed);
    }

    void RotateCameraWithScrollWheel()
    {
        if (!cameraTransform)
            return;

        if (!Input.GetKey(KeyCode.Mouse2))
            return;

        cameraTransform.PolarRotation(settingsDatas.cameraInvertVerticalWheelRotation ? -mouseDirection.y : mouseDirection.y, wheelRotationSpeed);

        if (!allowRotation)
            return;

        cameraTransform.AzimuthalRotation(settingsDatas.cameraInvertHorizontalWheelRotation ? -mouseDirection.x : mouseDirection.x, wheelRotationSpeed);
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
