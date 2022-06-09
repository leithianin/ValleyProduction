using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class CinematicShot : SphericalTransform
{
    [Header("ScriptableObject")]
    [SerializeField] public Camera _camera = default;
    public GameObject ui;
    public bool soloCamera = false;

    public bool isTraveling = false;
    public bool cinematic = false;
    public float speed = 1.0f;

    public bool isRotating = false;
    public bool clockwise = true;
    public float rotationSpeed = 1.0f;

    public AnimationCurve speedOverTime = default;

    public bool useCustomDuration = false;
    public float duration = 0.0f;

    [SerializeField] private Transform originTarget = default;
    [SerializeField] private GameObject parentGameObject = default;
    [SerializeField] private GameObject originGameObject = default;
    [SerializeField] private string fileName;
    [SerializeField] private GameObject userInterface = default;
    public string FileName => fileName;
    public CameraData cameraData = default;
    private const string dataPath = "_Scripts/Camera/Data";
    public string DataPath => dataPath;

    private void Awake()
    {
        boxBoundariesCollider = GameObject.Find("Boundaries").GetComponent<BoxCollider>();
        sphereBoundariesCollider = GameObject.Find("Boundaries").GetComponent<SphereCollider>();
    }

    private void Update()
    {
        //UnlockCameraView();

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

        //WriteScriptableObject();
    }

    public void WriteScriptableObject()
    {
        if (cameraData != null)
        {
            cameraData.scene = SceneManager.GetActiveScene().name;

            cameraData.radius = Coordinates.x;
            cameraData.azimuthalAngle = Coordinates.y;
            cameraData.polarAngle = Coordinates.z;

            cameraData.verticalOffset = OriginVisualOffset;

            cameraData.cameraOriginPosition = origin.position;

            cameraData.isTraveling = isTraveling;

            cameraData.isRotating = isRotating;
            cameraData.clockwise = clockwise;
            cameraData.rotationSpeed = rotationSpeed;

            cameraData.speedOverTime = speedOverTime;

            cameraData.cinematic = cinematic;

            cameraData.useCustomDuration = useCustomDuration;
            cameraData.duration = duration;

            if (isTraveling)
            {
                cameraData.travelPosition = originTarget.position;
                cameraData.speed = speed;

            }
        }
        else
        {
            Debug.Log("No scirptableobject serialized");
        }
    }

    public void SoloCamera()
    {
        _camera.depth = soloCamera  ?  10.0f : 0.0f;
    }

    public void GetBoundaries()
    {
        boxBoundariesCollider = GameObject.Find("Boundaries").GetComponent<BoxCollider>();
        sphereBoundariesCollider = GameObject.Find("Boundaries").GetComponent<SphereCollider>();
    }

    protected override void DrawTargetInEditor()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(originTarget.position, 0.1f);
        Gizmos.DrawLine(origin.position, originTarget.position);
    }
}
