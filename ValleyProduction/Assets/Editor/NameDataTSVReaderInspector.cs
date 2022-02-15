using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ReadNameDataTSV))]
public class NameDataTSVReaderInspector : Editor
{
    ReadNameDataTSV targetScript;

    private void OnEnable()
    {
        targetScript = (target as ReadNameDataTSV);
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Update DataName"))
        {
            targetScript.ReadTSVFile();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void OnDisable()
    {

    }
}
