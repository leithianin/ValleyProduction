using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[ExecuteAlways, DisallowMultipleComponent]
public class SphericalTransform : MonoBehaviour
{
    [SerializeField] private Transform origin = default;

    [Space(10)]
    [Header("Object Spherical Coordinates")]
    [SerializeField, Tooltip("Object Spherical Transform coordinates, x = Radius, y = Azimuthal Angle, z = Polar Angle")] 
    private Vector3 coordinates = default;
    [Space(10)]
    [SerializeField, Tooltip("In degrees")] private float verticalOffset = 0.5f;
    [SerializeField] private float minPolarValue = 0.0f;
    [SerializeField] private float maxPolarValue = 100.0f;

    [Header("Radius values")]
    [SerializeField] private float minRadiusValue = 1.0f;
    [SerializeField] private float maxRadiusValue = 30.0f;

    [Header("Offset")]
    [SerializeField, Range(0f, 5f)] private float originVisualOffset;

    [SerializeField, ReadOnly] private bool belowTerrain;

    private Vector3 cameraTarget = default;
    private Vector3 originStrartPosition;

    public Transform Origin
    {
        get
        {
            return origin;
        }
    }
    public Vector3 OiriginStartPosition { get; set; }

    private Vector3 touchDown = default;

    private void Update()
    {
        TestHeight();
        SetOriginHeight();
        SetOriginForward();
        SetCameraTarget();
        SetTargetForward();
    }

    private void LateUpdate()
    {
        ConstraintAngles();
        ConvertCameraTargetTransformIntoCarthesianCoords();
    }

    #region Controls

    public void AzimuthalRotation(float rotationValue, float speed)
    {
        if (rotationValue < -1f || rotationValue > 1f)
            Debug.LogWarning("Rotation Value is not set correctly");

        coordinates.y += rotationValue * speed * Time.deltaTime;
    }

    public void PolarRotation(float rotationValue, float speed)
    {
        if (rotationValue < -1f || rotationValue > 1f)
            Debug.LogWarning("Rotation Value is not set correctly");

        coordinates.z += rotationValue * speed * Time.deltaTime;
    }

    public void ChangeLength(float deltaMagnitude, float scrollingSpeed)
    {
        //float target = coordinates.x + deltaMagnitude * scrollingSpeed * Time.deltaTime;
        //coordinates.x = Mathf.Lerp(coordinates.x, target, 0.1f);
        coordinates.x += deltaMagnitude * scrollingSpeed * Time.deltaTime;
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
        if (Physics.Raycast(transform.position + Vector3.up * 1000f, Vector3.down, out hit, 5000.0f))
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

    public void MoveOrigin(float xInput, float yInput, float speed)
    {
        origin.position += Vector3.Normalize(origin.forward * yInput + origin.right * xInput) * speed * (coordinates.x / 5) * Time.deltaTime;
    }

    public void MoveOriginFromStartPosition(Vector3 startPos, Vector3 movingVector)
    {
        origin.position = startPos + movingVector;
    }

    void SetOriginHeight()
    {
        Debug.DrawLine(origin.position + Vector3.up * 1000f, origin.position + Vector3.down * 5000.0f, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(origin.position + Vector3.up * 1000f, Vector3.down, out hit, 5000.0f))
        {
            Debug.DrawLine(origin.position + Vector3.up * 1000.0f, origin.position + Vector3.up * 1000.0f + Vector3.down * hit.distance, Color.green);
            origin.position = hit.point;
        }
    }

    void SetCameraTarget()
    {
        cameraTarget = transform.position;
    }

    void SetOriginForward()
    {
        origin.forward = GetOriginForwardVector();
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
}
