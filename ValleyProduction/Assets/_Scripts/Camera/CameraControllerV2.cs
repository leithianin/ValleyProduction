using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerV2 : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float t = 0.5f;
    [SerializeField] private float tClampMin = default;
    [SerializeField] private float tClampMax = default;
    [SerializeField] private float movingSpeed = default;
    [SerializeField] private float minDistToEnviro = default;
    [SerializeField] private bool cameraEdgeScorlling = default;

    [Header("Space Boundaries")]
    [SerializeField] private float minCameraHeight = default;
    [SerializeField] private float maxCameraHeight = default;

    [Header("Rotation Parameters")]
    [SerializeField] private float rotationSpeed = default;
    [SerializeField, Range(0.0f, 1.0f)] private float rotationSmoothingValue = default;

    [Header("Scrolling Wheel Parameters")]
    [SerializeField] private float scrollingSpeed = default;
    [SerializeField] private float movementWithScrollingWheelSmoothingValue = default;


    [Header("References")]
    [SerializeField] private Transform cameraViewXZTarget = default;
    [SerializeField] private Transform cameraViewTarget = default;
    [SerializeField] private Transform cameraPosTarget = default;
    [SerializeField] private Transform cameraAnchorXZ = default;
    [SerializeField] private Transform cameraAnchorY = default;
    [SerializeField] private CameraBehaviour cameraBehaviour = default;

    private void Awake()
    {
        cameraBehaviour.minDistToEnviro = minDistToEnviro;
        cameraBehaviour.rotationSpeed = rotationSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        MoveXZ(cameraViewXZTarget);
        ObjectPositionLerp(cameraViewTarget, cameraViewXZTarget);

        RotateYAxis(cameraViewXZTarget);
        RotateYAxis(cameraViewTarget);

        GetScrolling();
        MoveY(cameraAnchorY);

        MoveCameraPositionTarget();
    }

    //Move the camera parent in the player's inputs direction
    void MoveXZ(Transform obj)
    {
        Vector3 dir = obj.forward * Input.GetAxisRaw("Vertical") + obj.right * Input.GetAxisRaw("Horizontal");
        dir.Normalize();

        if (MouseCollideWithScreenBorders() && !Input.GetKey(KeyCode.Mouse2) && cameraEdgeScorlling)
        {
            Vector2 mouseDirection = GetMouseDirection();
            dir = obj.forward * mouseDirection.y + obj.right * mouseDirection.x;
            dir.Normalize();
        }
        cameraBehaviour.direction = dir;
        obj.position = obj.position + dir * movingSpeed * (t * 5) * Time.deltaTime;

    }

    void ObjectPositionLerp(Transform obj, Transform target)
    {
        obj.position = Vector3.Lerp(obj.position, target.position, 0.1f);
    }

    //Move the camera position target depending on the position of the others anchors
    void MoveCameraPositionTarget()
    {
        cameraPosTarget.position = BezierScript.QuadraticCurve(cameraViewTarget.position, cameraAnchorXZ.position, cameraAnchorY.position, t);
    }

    //Rotate the camera around the camera view target
    //Rotate with keyboard inputs or scrolling wheel
    void RotateYAxis(Transform obj)
    {
        if (Input.GetKey(KeyCode.A))
        {
            obj.Rotate(0.0f, rotationSpeed, 0.0f, Space.Self);
        }
        if (Input.GetKey(KeyCode.E))
        {
            obj.Rotate(new Vector3(0, -rotationSpeed, 0));
        }
        if (Input.GetKey(KeyCode.Mouse2))
        {
            float temp = Mathf.Lerp(0, -rotationSpeed * Input.GetAxis("Mouse X") * 10f, rotationSmoothingValue);
            obj.Rotate(0.0f, temp, 0.0f, Space.Self);

        }
    }

    //Move the Y axis anchor, allowing to raise the highest point reachable the camera
    void MoveY(Transform obj)
    {
        if (Input.GetKey(KeyCode.Mouse2))
        {
            float temp = Mathf.Clamp(Mathf.Lerp(obj.transform.position.y, obj.transform.position.y + Input.GetAxis("Mouse Y") * 10f, movementWithScrollingWheelSmoothingValue), minCameraHeight, maxCameraHeight);
            obj.transform.position = new Vector3(obj.transform.position.x, temp, obj.transform.position.z);
        }
    }

    //Change the t value for the quadratic movement of the camera position target
    void GetScrolling()
    {
        t = t - Input.GetAxis("Mouse ScrollWheel");
        t = Mathf.Clamp(t, tClampMin, tClampMax);
    }

    //Check if the mouse collide with on edge of the screen
    bool MouseCollideWithScreenBorders()
    {
        Vector3 mousePosition = Input.mousePosition;
        if (mousePosition.x >= Screen.width || mousePosition.y >= Screen.height || mousePosition.x <= 0.0f || mousePosition.y <= 0.0f)
        {
            return true;
        }
        return false;
    }

    //Get the vector between the mouse position and the center of the screen
    Vector2 GetMouseDirection()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Vector2 mouseDirection = new Vector2(mousePosition.x - screenCenter.x, mousePosition.y - screenCenter.y);
        mouseDirection.Normalize();
        return mouseDirection;
    }
}
