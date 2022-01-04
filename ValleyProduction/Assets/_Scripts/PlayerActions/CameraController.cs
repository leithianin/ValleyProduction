using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float axisSpeed;
    [SerializeField] private Transform cameraTransform;

    [SerializeField] private AnimationCurve cameraSpeedCoef;
    [SerializeField] private AnimationCurve decelerationCurve;
    [SerializeField] private float decelerationSpeed = 0.3f;
    [SerializeField] private float angleByScroll = 10f;
    [SerializeField] private float angleLimitUp = -70f;
    [SerializeField] private float angleLimitDown = -10f;
    [SerializeField] private float positionLimitDown = 10f;
    [SerializeField] private float positionLimitUp = 40f;


    [SerializeField] private Rigidbody rbody;
    private Vector3 moveInput;

    private float zoomAcceleration;
    private float currentAngle = 70f;
    private float scrollSpeed;
    private float currentDeceleration = 0;

    private float lerpTarget = 0;
    private float startLerp = 0;

    private float ZoomPercent => (currentAngle - angleLimitDown) / (angleLimitUp - angleLimitDown);

    private void Awake()
    {
        if (rbody == null)
        {
            Debug.LogError("Rigidbody2D is NULL");
        }

        currentAngle = angleLimitUp;
    }

    private void Start()
    {
        transform.position = new Vector3(transform.position.x, positionLimitUp + positionLimitDown, transform.position.z);

        cameraTransform.localEulerAngles = new Vector3(angleLimitUp, cameraTransform.localEulerAngles.y, cameraTransform.localEulerAngles.z);
    }

    private void OnEnable()
    {
        PlayerInputManager.OnKeyMove += MoveCamera;
        PlayerInputManager.OnMouseScroll += UpdateCameraZoomSpeed;
    }

    private void OnDisable()
    {
        PlayerInputManager.OnKeyMove -= MoveCamera;
        PlayerInputManager.OnMouseScroll -= UpdateCameraZoomSpeed;
    }

    private void Update()
    {
        UpdateCameraZoom();
    }

    private void MoveCamera(Vector2 direction)
    {
        moveInput = new Vector3(direction.x, 0, direction.y);
        rbody.velocity = moveInput * (axisSpeed * cameraSpeedCoef.Evaluate(ZoomPercent));
    }

    private void UpdateCameraZoomSpeed(float newScrollSpeed)
    {
        scrollSpeed = newScrollSpeed * 1000f;
    }

    private void UpdateCameraZoom()
    { 
        if (scrollSpeed != 0)
        {
            currentDeceleration = 1;

            startLerp = currentAngle;
            if (scrollSpeed < 0)
            {
                lerpTarget = angleByScroll + startLerp;
                if (lerpTarget > angleLimitUp)
                {
                    lerpTarget = angleLimitUp;
                }
            }
            else if (scrollSpeed > 0)
            {
                lerpTarget = -angleByScroll + startLerp;
                if (lerpTarget < angleLimitDown)
                {
                    lerpTarget = angleLimitDown;
                }
            }
        }

        scrollSpeed = 0;

        if (currentDeceleration != 0)
        {
            zoomAcceleration = decelerationCurve.Evaluate(1 - currentDeceleration);
            currentAngle = Mathf.Lerp(startLerp, lerpTarget, zoomAcceleration);

            cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x, CalculatePosition(), cameraTransform.localPosition.z);
            cameraTransform.localEulerAngles = new Vector3(currentAngle, cameraTransform.localEulerAngles.y, cameraTransform.localEulerAngles.z);

            currentDeceleration -= decelerationSpeed * Time.deltaTime;
            if (currentDeceleration < 0)
            {
                currentDeceleration = 0;
            }
        }
    }

    private float CalculatePosition()
    {
        return ((currentAngle - angleLimitDown) / (angleLimitUp - angleLimitDown)) * positionLimitUp + positionLimitDown;
    }
}
