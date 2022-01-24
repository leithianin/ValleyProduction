using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float axisSpeed;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform lookTarget;

    [SerializeField] private AnimationCurve cameraSpeedCoef;
    [SerializeField] private AnimationCurve decelerationCurve;
    [SerializeField] private float decelerationSpeed = 0.3f;
    [SerializeField] private float zoomPercentByScroll = 10f;
    [SerializeField] private float distanceFromTerrain = 10f;
    [SerializeField] private Vector2 positionLimitDown = new Vector2(0f, 10f);
    [SerializeField] private Vector2 positionLimitUp = new Vector2(-20f, 40f);


    [SerializeField] private Rigidbody rbody;
    private Vector3 moveInput;

    private float zoomAcceleration;
    private float zoomLevel;
    private float scrollSpeed;
    private float currentDeceleration = 0;

    private float lerpTarget = 0;
    private float startLerp = 0;

    private float ZoomPercent => zoomLevel / 100f;

    [SerializeField] private Terrain mainTerrain;

    private void Awake()
    {
        if (rbody == null)
        {
            Debug.LogError("Rigidbody2D is NULL");
        }

        zoomLevel = 100;
    }

    private void Start()
    {
        cameraTransform.localPosition = CalculatePosition(ZoomPercent);

        cameraTransform.forward = lookTarget.position - cameraTransform.position;

        mainTerrain = Terrain.activeTerrains[0];
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

    private Vector3 CalculatePosition(float percent)
    {
        return new Vector3(transform.forward.x * (positionLimitUp.x - positionLimitDown.x) * percent + positionLimitDown.x,
                           (positionLimitUp.y - positionLimitDown.y) * percent + positionLimitDown.y,
                           transform.forward.z * (positionLimitUp.x - positionLimitDown.x) * percent + positionLimitDown.x);
    }

    private void MoveCamera(Vector2 direction)
    {
        float yPosition = mainTerrain.SampleHeight(rbody.transform.position) - (distanceFromTerrain + rbody.transform.position.y);

        moveInput = new Vector3(direction.x, yPosition, direction.y);
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

            startLerp = zoomLevel;
            if (scrollSpeed < 0)
            {
                lerpTarget = zoomPercentByScroll + startLerp;
                if (lerpTarget > 100)
                {
                    lerpTarget = 100;
                }
            }
            else if (scrollSpeed > 0)
            {
                lerpTarget = -zoomPercentByScroll + startLerp;
                if (lerpTarget < 0)
                {
                    lerpTarget = 0;
                }
            }
        }

        scrollSpeed = 0;

        if (currentDeceleration != 0)
        {
            zoomAcceleration = decelerationCurve.Evaluate(1 - currentDeceleration);

            zoomLevel = Mathf.Lerp(startLerp, lerpTarget, zoomAcceleration);

            cameraTransform.localPosition = CalculatePosition(ZoomPercent);

            cameraTransform.forward = lookTarget.position - cameraTransform.position;

            currentDeceleration -= decelerationSpeed * Time.deltaTime;
            if (currentDeceleration < 0)
            {
                currentDeceleration = 0;
            }
        }
    }
}
