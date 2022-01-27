using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector4 cameraBounds = new Vector4(0, 250, 0, 250);

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

    private float zoomAcceleration = 0;
    private float zoomLevel = 0;
    private float scrollSpeed = 0;
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
        if(new Vector2(rbody.velocity.x, rbody.velocity.z).magnitude < 1f)
        {
            rbody.velocity = Vector3.zero;
        }
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

        float speed = axisSpeed * cameraSpeedCoef.Evaluate(ZoomPercent);

        moveInput = new Vector3(direction.x, yPosition, direction.y);
        rbody.velocity = moveInput * speed;

        if (rbody.position.x > cameraBounds.y)
        {
            rbody.transform.position = new Vector3(cameraBounds.y, rbody.transform.position.y, rbody.transform.position.z);
        }
        else if (rbody.position.x < cameraBounds.x)
        {
            rbody.transform.position = new Vector3(cameraBounds.x, rbody.transform.position.y, rbody.transform.position.z);
        }

        if (rbody.position.z > cameraBounds.w)
        {
            rbody.transform.position = new Vector3(rbody.transform.position.x, rbody.transform.position.y, cameraBounds.w);
        }
        else if(rbody.position.z < cameraBounds.z)
        {
            rbody.transform.position = new Vector3(rbody.transform.position.x, rbody.transform.position.y, cameraBounds.z);
        }

        if(rbody.transform.position.y < distanceFromTerrain)
        {
            Debug.Log("Allo");
            rbody.transform.position = new Vector3(rbody.transform.position.x, distanceFromTerrain, rbody.transform.position.z);
        }

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
