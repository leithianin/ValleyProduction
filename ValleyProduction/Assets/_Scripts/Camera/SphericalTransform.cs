using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;

[ExecuteAlways, DisallowMultipleComponent]
public class SphericalTransform : MonoBehaviour
{

    [SerializeField] protected Transform origin = default;
    [SerializeField] private Transform originLookAtTarget = default;

    [Space(10)]
    [Header("Object Spherical Coordinates")]
    [SerializeField, Tooltip("Object Spherical Transform coordinates, x = Radius, y = Azimuthal Angle, z = Polar Angle")] 
    private Vector3 coordinates = default;
    public Vector3 Coordinates => coordinates;
    [Space(10)]

    [Header("Reference Values")]
    [SerializeField] private float referencePolarAngle;
    [SerializeField] private float referenceAzimuthalAngle;
    [SerializeField] private float resetPositionSpeed;
    public float ReferencePolarAngle => referencePolarAngle;
    public float ReferenceAzimuthalAngle => referenceAzimuthalAngle;
    public float ResetPositionSpeed => resetPositionSpeed;

    [Header("Moving Constraints")]
    [SerializeField] private LayerMask layerMask = default;
    [SerializeField] protected Collider boundariesCollider = default;

    [Header("Polar Values")]
    [SerializeField, Tooltip("In degrees")] private float verticalOffset = 0.5f;
    [SerializeField] private float minPolarValue = 0.0f;
    [SerializeField] private float maxPolarValue = 100.0f;

    [Header("Radius Values")]
    [SerializeField] private float minRadiusValue = 1.0f;
    [SerializeField] private float maxRadiusValue = 30.0f;

    [Header("LookAt")]
    [SerializeField, Range(0,1)] private float lookAtLerpValue = 0.1f;

    [Header("Offset")]
    [SerializeField, Range(0f, 10f)] private float originVisualOffset;
    public float OriginVisualOffset { get => originVisualOffset; set => originVisualOffset = value; }

    [Header("Debug")]
    [SerializeField] private bool drawGroundDebug = true;
    [SerializeField] private Mesh debugMesh = default;

    [SerializeField, ReadOnly] private bool belowTerrain;

    public static Action<float> OnMouseWheel;

    protected Vector3 cameraTarget = default;

    private Vector3 touchDown = default;

    private Transform target;

    private void Update()
    {
        TestHeight();
        SetOriginHeight();
        SetOriginForward();
        SetCameraTarget();
        //SetTargetForward();
        MoveOriginLookAtTarget();

        if(target != null)
        {
            origin.position = target.position;
        }
    }

    private void LateUpdate()
    {
        ConstraintAngles();
        ConstraintOriginPosition();
        ConvertCameraTargetTransformIntoCarthesianCoords();
    }


    #region Controls

    public void AzimuthalRotation(float rotationValue, float speed)
    {
        if (rotationValue < -1f || rotationValue > 1f)
            Debug.LogWarning("Rotation Value is not set correctly");

        coordinates.y += rotationValue * speed * Time.unscaledDeltaTime;

        if (rotationValue != 0)
        {
            PlayerInputManager.GetOnMouseWheelDown?.Invoke(rotationValue);
        }
    }

    public void PolarRotation(float rotationValue, float speed)
    {
        if (rotationValue < -1f || rotationValue > 1f)
            Debug.LogWarning("Rotation Value is not set correctly");

        coordinates.z += rotationValue * speed * Time.unscaledDeltaTime;

        if (rotationValue != 0)
        {
            PlayerInputManager.GetOnMouseWheelDown?.Invoke(rotationValue);
        }
    }

    public void ChangeLength(float deltaMagnitude, float scrollingSpeed)
    {
        coordinates.x += deltaMagnitude * scrollingSpeed * Time.unscaledDeltaTime;
    }

    public void SetLength(float newValue)
    {
        coordinates.x = newValue;
    }

    #endregion

    public Vector3 SphericalToCarthesian(Vector3 sphericalCoords)
    {
        Vector3 cartCoords;

        float x = sphericalCoords.x * Mathf.Cos(sphericalCoords.y * Mathf.Deg2Rad) * Mathf.Sin(sphericalCoords.z * Mathf.Deg2Rad);
        float z = sphericalCoords.x * Mathf.Sin(sphericalCoords.y * Mathf.Deg2Rad) * Mathf.Sin(sphericalCoords.z * Mathf.Deg2Rad);
        float y = sphericalCoords.x * Mathf.Cos(sphericalCoords.z * Mathf.Deg2Rad);
        cartCoords = new Vector3(x, y, z);

        return cartCoords;
    }

    protected void ConvertCameraTargetTransformIntoCarthesianCoords()
    {
        transform.position = SphericalToCarthesian(coordinates) + origin.position;
    }

    protected void TestHeight()
    {
        //Debug.DrawLine(transform.position + Vector3.up * 1000f, transform.position + Vector3.down*5000.0f, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 1000f, Vector3.down, out hit, 5000.0f, layerMask))
        {
            //Debug.DrawLine(transform.position + Vector3.up * BoundariesCollider.bounds.extents.y, transform.position + Vector3.up * BoundariesCollider.bounds.extents.y + Vector3.down * hit.distance, Color.green);
            touchDown = hit.point;

            belowTerrain = transform.position.y <= hit.point.y;
        }

        else
        {
            touchDown = Vector3.zero;

            belowTerrain = false;
        }
    }

    #region Origin

    public void SetOrigin(Vector3 newPos)
    {
        origin.position = new Vector3(newPos.x, origin.position.y, newPos.z);
    }

    public void MoveOrigin(float xInput, float yInput, float speed)
    {
        Vector3 savePosition = origin.position;
        origin.position += Vector3.Normalize(origin.forward * yInput + origin.right * xInput) * speed * (coordinates.x / 5) * Time.unscaledDeltaTime;

        if(origin.position != savePosition)
        {
            CameraManager.OnCameraMove?.Invoke();
        }
    }

    public IEnumerator MoveCameraOriginToCustomTarget(Transform target, float speed)
    {
        Vector3 startPos = origin.position;
        float referenceTime = Vector3.Distance(startPos, target.position) / speed;

        for (float time = referenceTime; time > 0; time -= Time.unscaledDeltaTime)
        {
            origin.position = Vector3.Lerp(target.position, startPos, time / referenceTime);
            yield return null;
        }
    }

    public void MoveOriginFromStartPosition(Vector3 startPos, Vector3 movingVector)
    {
        origin.position = startPos + movingVector;
    }

    public void SetOriginPosition(Vector3 posToGo)
    {
        origin.position = new Vector3(posToGo.x, origin.position.y, posToGo.z);
        originLookAtTarget.position = origin.position;
    }

    public Vector3 GetOriginPosition()
    {
        return origin.position;
    }

    protected void MoveOriginLookAtTarget()
    {
        originLookAtTarget.position = Vector3.Lerp(originLookAtTarget.position, origin.position, lookAtLerpValue);
    }


    protected void SetOriginForward()
    {
        origin.forward = GetOriginForwardVector();
    }

    protected void SetOriginHeight()
    {
        //Debug.DrawLine(origin.position + Vector3.up * 1000f, origin.position + Vector3.down * 5000.0f, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(origin.position + Vector3.up * 1000f, Vector3.down, out hit, 5000.0f, layerMask))
        {
            //Debug.DrawLine(origin.position + Vector3.up * BoundariesCollider.bounds.extents.y, origin.position + Vector3.up * BoundariesCollider.bounds.extents.y + Vector3.down * hit.distance, Color.green);
            origin.position = hit.point;
        }
    }
    #endregion

    protected void SetCameraTarget()
    {
        cameraTarget = transform.position;
    }

    public void SetCameraTarget(Transform _target = null)
    {
        target = _target;
    }

    public void SetTargetForward()
    {
        transform.forward = origin.position - transform.position + new Vector3(0.0f, originVisualOffset, 0.0f);
    }

    public Vector3 GetCameraTarget()
    {
        return cameraTarget;
    }

    public float GetTargetDistanceToOrigin()
    {
        return coordinates.x;
    }

    protected Vector3 GetOriginForwardVector()
    {
        return Vector3.Normalize(new Vector3(origin.position.x - transform.position.x, 0.0f, origin.position.z - transform.position.z));
    }

    #region Set Spherical Coordinates
    public void SetRadius(float value)
    {
        coordinates.x = value;
    }

    public void SetPolarAngle(float value)
    {
        coordinates.z = value;
    }

    public void SetAzimuthalAngle(float value)
    {
        coordinates.y = value;
    }

    public void MoveCameraOverTime(float targetRadius, float targetAzimuthalAngle, float targetPolarAngle, float speed)
    {
        StartCoroutine(ChangeRadiusOverTime(targetRadius, speed));
        StartCoroutine(ChangeAzimuthalAngleOverTime(targetAzimuthalAngle, speed));
        StartCoroutine(ChangePolarAngleOverTime(targetPolarAngle, speed));

    }
    public void MoveCameraOverTime(float targetRadius, float radiusSpeed, float targetAzimuthalAngle, float azimuthSpeed, float targetPolarAngle, float polarSpeed)
    {
        StartCoroutine(ChangeRadiusOverTime(targetRadius, radiusSpeed));
        StartCoroutine(ChangeAzimuthalAngleOverTime(targetAzimuthalAngle, azimuthSpeed));
        StartCoroutine(ChangePolarAngleOverTime(targetPolarAngle, polarSpeed));

    }

    public IEnumerator ChangeRadiusOverTime(float targetRadius, float speed)
    {
        float startRadius = coordinates.x;
        float referenceTime = Mathf.Abs(startRadius - targetRadius) / speed;

        for (float time = 0.0f; time < referenceTime; time += Time.unscaledDeltaTime)
        {
            coordinates.x = Mathf.Lerp(startRadius, targetRadius, time / referenceTime);
            yield return null;
        }

        coordinates.x = targetRadius;
    }

    public IEnumerator ChangeAzimuthalAngleOverTime(float targetAzimuthalAngle, float speed)
    {
        float startAzimuth = coordinates.y;
        float referenceTime = Mathf.Abs(startAzimuth - targetAzimuthalAngle) / speed;

        for (float time = 0.0f; time < referenceTime; time += Time.unscaledDeltaTime)
        {
            coordinates.y = AngleLerp(startAzimuth, targetAzimuthalAngle, time / referenceTime);
            yield return null;
        }

        coordinates.y = targetAzimuthalAngle;
    }

    public IEnumerator ChangePolarAngleOverTime(float targetPolarAngle, float speed)
    {
        float startPolar = coordinates.z;
        float referenceTime = Mathf.Abs(startPolar - targetPolarAngle) / speed;

        for (float time = 0.0f; time < referenceTime; time += Time.unscaledDeltaTime)
        {
            coordinates.z = AngleLerp(startPolar, targetPolarAngle, time / referenceTime);
            yield return null;
        }

        coordinates.z = targetPolarAngle;
    }

    private float AngleLerp(float a, float b, float t)
    {
        float dist = (((b - a) + 180) % 360f) - 180f;
        return (a + dist * t) % 360;
    }
    #endregion

    #region Constraints
    protected void ConstraintAngles()
    {
        if (coordinates.y >= 360f)
        {
            coordinates.y = coordinates.y - 360f;
        }
        else if (coordinates.y < 0f)
        {
            coordinates.y = coordinates.y + 360f;
        }

        coordinates.z = Mathf.Clamp(coordinates.z,minPolarValue, Vector3.Angle(touchDown - origin.position, origin.up) - verticalOffset);
        coordinates.x = Mathf.Clamp(coordinates.x, minRadiusValue, maxRadiusValue);
    }

    protected void ConstraintOriginPosition()
    {
        if (boundariesCollider != null)
        {
            float xOriginClamped = Mathf.Clamp(origin.position.x, boundariesCollider.bounds.center.x - boundariesCollider.bounds.extents.x, boundariesCollider.bounds.center.x + boundariesCollider.bounds.extents.x);
            float zOriginClamped = Mathf.Clamp(origin.position.z, boundariesCollider.bounds.center.z - boundariesCollider.bounds.extents.z, boundariesCollider.bounds.center.z + boundariesCollider.bounds.extents.z);
            origin.position = new Vector3(xOriginClamped, origin.position.y, zOriginClamped);
        }
    }
    #endregion

    #region Debug
    private void OnDrawGizmos()
    {
        if (!origin)
            return;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(origin.position, transform.position);

        Gizmos.color = Color.black;
        Gizmos.DrawRay(origin.position, origin.forward);

        if(touchDown != Vector3.zero)
        {
            
            if (drawGroundDebug)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireMesh(debugMesh, touchDown, Quaternion.identity, Vector3.one * 0.5f);
            }
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(touchDown, 0.3f);
        }

        Gizmos.color = Color.green;
        Gizmos.DrawLine(touchDown, cameraTarget);

        
        if (drawGroundDebug)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireMesh(debugMesh, origin.position, Quaternion.identity, Vector3.one * 0.5f);
        }
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(origin.position, 0.3f);
        Gizmos.DrawLine(origin.position, origin.position + origin.forward * 2);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(cameraTarget, 0.25f);

        DrawTargetInEditor();

    }

    protected virtual void DrawTargetInEditor() { }
    #endregion
}