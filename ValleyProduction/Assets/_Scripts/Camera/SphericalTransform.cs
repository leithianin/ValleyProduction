using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

[ExecuteAlways, DisallowMultipleComponent]
public class SphericalTransform : MonoBehaviour
{
    [SerializeField] private Transform origin = default;
    [SerializeField] private Transform originLookAtTarget = default;

    [Space(10)]
    [Header("Object Spherical Coordinates")]
    [SerializeField, Tooltip("Object Spherical Transform coordinates, x = Radius, y = Azimuthal Angle, z = Polar Angle")] 
    private Vector3 coordinates = default;
    [Space(10)]
    [Header("Moving Constraints")]
    [SerializeField] private LayerMask layerMask = default;
    [SerializeField] private Collider BoundariesCollider = default;

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
    [SerializeField, Range(0f, 5f)] private float originVisualOffset;

    [SerializeField, ReadOnly] private bool belowTerrain;

    private Vector3 cameraTarget = default;

    private Vector3 touchDown = default;

    private void Update()
    {
        TestHeight();
        SetOriginHeight();
        SetOriginForward();
        SetCameraTarget();
        SetTargetForward();
        MoveOriginLookAtTarget();
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
    }

    public void PolarRotation(float rotationValue, float speed)
    {
        if (rotationValue < -1f || rotationValue > 1f)
            Debug.LogWarning("Rotation Value is not set correctly");

        coordinates.z += rotationValue * speed * Time.unscaledDeltaTime;
    }

    public void ChangeLength(float deltaMagnitude, float scrollingSpeed)
    {
        coordinates.x += deltaMagnitude * scrollingSpeed * Time.unscaledDeltaTime;
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

    void ConvertCameraTargetTransformIntoCarthesianCoords()
    {
        transform.position = SphericalToCarthesian(coordinates) + origin.position;
    }

    void TestHeight()
    {
        Debug.DrawLine(transform.position + Vector3.up * 1000f, transform.position + Vector3.down*5000.0f, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 1000f, Vector3.down, out hit, 5000.0f, layerMask))
        {
            Debug.DrawLine(transform.position + Vector3.up * 1000f, transform.position + Vector3.up * 1000f + Vector3.down * hit.distance, Color.green);
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
    public void MoveOrigin(float xInput, float yInput, float speed)
    {
        origin.position += Vector3.Normalize(origin.forward * yInput + origin.right * xInput) * speed * (coordinates.x / 5) * Time.unscaledDeltaTime;
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

    private void MoveOriginLookAtTarget()
    {
        originLookAtTarget.position = Vector3.Lerp(originLookAtTarget.position, origin.position, lookAtLerpValue);
    }


    void SetOriginForward()
    {
        origin.forward = GetOriginForwardVector();
    }

    void SetOriginHeight()
    {
        Debug.DrawLine(origin.position + Vector3.up * 1000f, origin.position + Vector3.down * 5000.0f, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(origin.position + Vector3.up * 1000f, Vector3.down, out hit, 5000.0f, layerMask))
        {
            Debug.DrawLine(origin.position + Vector3.up * 1000.0f, origin.position + Vector3.up * 1000.0f + Vector3.down * hit.distance, Color.green);
            origin.position = hit.point;
        }
    }
    #endregion


    void SetCameraTarget()
    {
        cameraTarget = transform.position;
    }

    
    void SetTargetForward()
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

    Vector3 GetOriginForwardVector()
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

    public IEnumerator ChangeRadiusOverTime(float targetRadius, float speed)
    {
        float startRadius = coordinates.x;
        float referenceTime = Mathf.Abs(startRadius - targetRadius) / speed;

        for (float time = referenceTime; time > 0; time -= Time.unscaledDeltaTime)
        {
            coordinates.x = Mathf.Lerp(targetRadius, startRadius, time / referenceTime);
            yield return null;
        }
    }
    #endregion

    #region Constraints
    void ConstraintAngles()
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

    void ConstraintOriginPosition()
    {
        float xOriginClamped = Mathf.Clamp(origin.position.x, BoundariesCollider.bounds.center.x - BoundariesCollider.bounds.extents.x, BoundariesCollider.bounds.center.x + BoundariesCollider.bounds.extents.x);
        float zOriginClamped = Mathf.Clamp(origin.position.z, BoundariesCollider.bounds.center.z - BoundariesCollider.bounds.extents.z, BoundariesCollider.bounds.center.z + BoundariesCollider.bounds.extents.z);

        origin.position = new Vector3(xOriginClamped, origin.position.y, zOriginClamped);
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
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(touchDown, 0.5f);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(origin.position, new Vector3(1, 1, 1));

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(cameraTarget, 0.25f);

    }
    #endregion
}