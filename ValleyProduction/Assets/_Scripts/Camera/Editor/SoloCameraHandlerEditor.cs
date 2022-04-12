using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SoloCameraHandler))]
public class SoloCameraHandlerEditor : Editor
{
    SoloCameraHandler soloCameraHandler;
    static GameObject ui;

    private void OnEnable()
    {
        if (ui == null)
            ui = GameObject.Find("-UI-");
    }

    private void OnDisable()
    {
        UpdateSoloCamera();
        UpdateUI();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

    private void UpdateSoloCamera()
    {
        soloCameraHandler = (SoloCameraHandler)target;

        if (Selection.activeGameObject != soloCameraHandler.obj01 
            && Selection.activeGameObject != soloCameraHandler.obj02 
            && Selection.activeGameObject != soloCameraHandler.obj03)
        {
            soloCameraHandler.cinematicShot.soloCamera = false;
            soloCameraHandler.cinematicShot.SoloCamera();
        }
    }

    private void UpdateUI()
    {
        soloCameraHandler = (SoloCameraHandler)target;

        if (Selection.activeGameObject != soloCameraHandler.obj01
            && Selection.activeGameObject != soloCameraHandler.obj02
            && Selection.activeGameObject != soloCameraHandler.obj03)
        {
            if (!ui.activeSelf)
                ui.SetActive(true);
        }
    }
}
