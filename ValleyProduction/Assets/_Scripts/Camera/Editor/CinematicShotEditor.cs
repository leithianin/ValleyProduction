using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine.SceneManagement;

[CanEditMultipleObjects]
[CustomEditor(typeof(CinematicShot))]
public class CinematicShotEditor : Editor
{
    CinematicShot cinematicShot;

    bool advancedMode = false;
    GameObject ui;
    bool isUIDisabled;

    Color baseColor;

    SerializedProperty origin;
    SerializedProperty originTarget;
    SerializedProperty originLookAtTarget;

    SerializedProperty coordinates;
    SerializedProperty radius;
    SerializedProperty hazimuthalAngle;
    SerializedProperty polarAngle;

    SerializedProperty layerMask;
    SerializedProperty colliderToUse;
    SerializedProperty boxBoundariesCollider;
    SerializedProperty sphereBoundariesCollider;

    SerializedProperty verticalOffset;
    SerializedProperty minPolarValue;
    SerializedProperty maxPolarValue;

    SerializedProperty minRadiusValue;
    SerializedProperty maxRadiusValue;

    SerializedProperty lookAtLerpValue;

    SerializedProperty originVisualOffset;

    SerializedProperty useCustomDuration;
    SerializedProperty duration;

    SerializedProperty drawGroundDebug;
    SerializedProperty debugMesh;

    SerializedProperty fileName;

    SerializedProperty camera;
    SerializedProperty parentGameObject;
    SerializedProperty originGameObject;
    SerializedProperty cameraData;

    SerializedProperty isTraveling;
    SerializedProperty cinematic;
    SerializedProperty speed;

    SerializedProperty isRotating;
    SerializedProperty clockwise;
    SerializedProperty rotationSpeed;

    SerializedProperty speedOverTime;


    void OnEnable()
    {

        origin = serializedObject.FindProperty("origin");
        originTarget = serializedObject.FindProperty("originTarget");
        originLookAtTarget = serializedObject.FindProperty("originLookAtTarget");

        coordinates = serializedObject.FindProperty("coordinates");
        radius = serializedObject.FindProperty("coordinates.x");
        hazimuthalAngle = serializedObject.FindProperty("coordinates.y");
        polarAngle = serializedObject.FindProperty("coordinates.z");

        layerMask = serializedObject.FindProperty("layerMask");
        colliderToUse = serializedObject.FindProperty("colliderToUse");
        boxBoundariesCollider = serializedObject.FindProperty("boxBoundariesCollider");
        sphereBoundariesCollider = serializedObject.FindProperty("sphereBoundariesCollider");

        verticalOffset = serializedObject.FindProperty("originVisualOffset");
        minPolarValue = serializedObject.FindProperty("minPolarValue");
        maxPolarValue = serializedObject.FindProperty("maxPolarValue");

        minRadiusValue = serializedObject.FindProperty("minRadiusValue");
        maxRadiusValue = serializedObject.FindProperty("maxRadiusValue");

        lookAtLerpValue = serializedObject.FindProperty("lookAtLerpValue");

        originVisualOffset = serializedObject.FindProperty("originVisualOffset");

        drawGroundDebug = serializedObject.FindProperty("drawGroundDebug");
        debugMesh = serializedObject.FindProperty("debugMesh");

        fileName = serializedObject.FindProperty("fileName");

        camera = serializedObject.FindProperty("_camera");
        parentGameObject = serializedObject.FindProperty("parentGameObject");
        originGameObject = serializedObject.FindProperty("originGameObject");
        cameraData = serializedObject.FindProperty("cameraData");

        isTraveling = serializedObject.FindProperty("isTraveling");
        cinematic = serializedObject.FindProperty("cinematic");
        speed = serializedObject.FindProperty("speed");

        isRotating = serializedObject.FindProperty("isRotating");
        clockwise = serializedObject.FindProperty("clockwise");
        rotationSpeed = serializedObject.FindProperty("rotationSpeed");

        useCustomDuration = serializedObject.FindProperty("useCustomDuration");
        duration = serializedObject.FindProperty("duration");

        speedOverTime = serializedObject.FindProperty("speedOverTime");

        ui = GameObject.Find("-UI-");
        baseColor = GUI.backgroundColor;


    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        cinematicShot = (CinematicShot)target;


        GUILayout.BeginHorizontal("Type");
        GUI.backgroundColor = advancedMode ? baseColor : Color.yellow;
        if (GUILayout.Button("Normal"))
        {
            advancedMode = false;
        }
        GUI.backgroundColor = advancedMode ? Color.yellow : baseColor;
        if (GUILayout.Button("Advanced"))
        {
            advancedMode = true;
        }
        GUI.backgroundColor = baseColor;
        GUILayout.EndHorizontal();


        EditorGUILayout.Space();

        if (advancedMode)
            AdvandedMode();
        else
            NormalMode();

        //DrawDefaultInspector();

        serializedObject.ApplyModifiedProperties();

        cinematicShot.WriteScriptableObject();
        EditorUtility.SetDirty(cinematicShot.cameraData);
    }

    private void NormalMode()
    {
        GUILayout.BeginHorizontal("Options");
        GUI.backgroundColor =  cinematicShot.soloCamera ? Color.yellow : baseColor;
        if (GUILayout.Button("Solo Camera"))
        {
            cinematicShot.soloCamera = !cinematicShot.soloCamera;
            cinematicShot.SoloCamera();

        }

        GUI.backgroundColor = ui.activeSelf ? baseColor : Color.yellow;
        if (GUILayout.Button("Disable UI"))
        {
            DisableUI();
        }
        GUI.backgroundColor = baseColor;
        GUILayout.EndHorizontal();


        //EditorGUILayout.PropertyField(coordinates);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Cameras position values", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(radius, new GUIContent("Radius"));
        EditorGUILayout.PropertyField(hazimuthalAngle, new GUIContent("Horizontal Angle"));
        EditorGUILayout.PropertyField(polarAngle, new GUIContent("Vertical Angle"));

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(verticalOffset, new GUIContent("Vertical Angle Offset"));

        EditorGUILayout.Space();
        GUILayout.BeginHorizontal("Dolly");
        EditorGUILayout.PropertyField(isTraveling, new GUIContent("Camera Dolly"));
        if (cinematicShot.isTraveling)
        {
            EditorGUILayout.PropertyField(speed);
        }
        GUILayout.EndHorizontal();

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(isRotating, new GUIContent("Rotate"));
        if (cinematicShot.isRotating)
        {
            GUILayout.BeginHorizontal("Rotation");
            EditorGUILayout.PropertyField(clockwise);
            EditorGUILayout.PropertyField(rotationSpeed, new GUIContent("Speed"));
            GUILayout.EndHorizontal();
        }

        EditorGUILayout.Space();
        GUILayout.BeginHorizontal("Duration");
        EditorGUILayout.PropertyField(useCustomDuration, new GUIContent("Custom Duration"));
        if (cinematicShot.useCustomDuration)
            EditorGUILayout.PropertyField(duration);
        GUILayout.EndHorizontal();
        
        EditorGUILayout.PropertyField(speedOverTime);

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(cinematic, new GUIContent("Cinematic"));

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("ScriptableObject", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal("Scriptable");
        EditorGUILayout.PropertyField(fileName);
        if (GUILayout.Button("Create ScriptableObject"))
        {
            CreateScriptable();
        }
        GUILayout.EndHorizontal();

    }

    private void AdvandedMode()
    {
        GUI.backgroundColor = cinematicShot.soloCamera ? Color.yellow : baseColor;
        if (GUILayout.Button("Solo Camera"))
        {
            cinematicShot.soloCamera = !cinematicShot.soloCamera;
            cinematicShot.SoloCamera();
        }
        GUI.backgroundColor = baseColor;

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(colliderToUse);
        EditorGUILayout.PropertyField(boxBoundariesCollider);
        EditorGUILayout.PropertyField(sphereBoundariesCollider);
        if (GUILayout.Button("Get Boundaries"))
        {
            cinematicShot.GetBoundaries();
        }

        EditorGUILayout.PropertyField(origin);
        EditorGUILayout.PropertyField(originTarget);
        EditorGUILayout.PropertyField(originLookAtTarget);

        EditorGUILayout.PropertyField(coordinates);

        EditorGUILayout.PropertyField(layerMask);
        EditorGUILayout.PropertyField(boxBoundariesCollider);
        EditorGUILayout.PropertyField(sphereBoundariesCollider);

        EditorGUILayout.PropertyField(verticalOffset);
        EditorGUILayout.PropertyField(minPolarValue);
        EditorGUILayout.PropertyField(maxPolarValue);

        EditorGUILayout.PropertyField(minRadiusValue);
        EditorGUILayout.PropertyField(maxRadiusValue);

        EditorGUILayout.PropertyField(lookAtLerpValue);

        EditorGUILayout.PropertyField(originVisualOffset);

        EditorGUILayout.PropertyField(maxRadiusValue);

        EditorGUILayout.PropertyField(cinematic);

        EditorGUILayout.PropertyField(drawGroundDebug);
        EditorGUILayout.PropertyField(debugMesh);
        EditorGUILayout.PropertyField(camera);
        EditorGUILayout.PropertyField(parentGameObject);
        EditorGUILayout.PropertyField(originGameObject);

        EditorGUILayout.PropertyField(fileName);
        EditorGUILayout.PropertyField(cameraData);
        if (GUILayout.Button("Create ScriptableObject"))
        {
            CreateScriptable();
        } 
    }

    private void CreateScriptable()
    {
        //CameraData cameraData = cinematicShot.cameraData;

        if (cinematicShot.cameraData == null)
        {
            if (!AssetDatabase.IsValidFolder("Assets/_Scripts/Camera/Data"))
            {
                Debug.Log("Path did not exist & has been created");
                AssetDatabase.CreateFolder("Assets/_Scripts/Camera", "Data");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            var tempData = Editor.CreateInstance<CameraData>();
            AssetDatabase.CreateAsset(tempData, "Assets/" + cinematicShot.DataPath + "/" + cinematicShot.FileName + ".asset");

            tempData.scene = SceneManager.GetActiveScene().name;
            cinematicShot.cameraData = tempData;
        }
        else
        {
            Debug.Log("ScriptableObject already created");
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = cinematicShot.cameraData;
        }
    }

    private void DisableUI()
    {
        ui.SetActive(!ui.activeSelf);
    }

}
