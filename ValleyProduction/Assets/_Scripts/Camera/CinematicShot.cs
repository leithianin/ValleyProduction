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
        boundariesCollider = GameObject.Find("Boundaries").GetComponent<Collider>();
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

        //WriteScirptableObject();
    }

    public void WriteScirptableObject()
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
            cameraData.cinematic = cinematic;

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

    protected override void DrawTargetInEditor()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(originTarget.position, 0.1f);
        Gizmos.DrawLine(origin.position, originTarget.position);
    }
}
