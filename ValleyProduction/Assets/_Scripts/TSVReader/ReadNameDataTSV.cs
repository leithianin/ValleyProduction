using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;

public class ReadNameDataTSV : MonoBehaviour
{
    string[] lines;         //List of all tab's lines
    string[] blocks;        //List of each line's columns

    public NameScriptable nameScript;
    public NameScriptable pathNameScript;

    public bool Toggle;

    public void ReadTSVFile()
    {
        SetVisitorsName();
        SetPathName();

        Toggle = true;
    }

    [HideIf("Toggle")]
    [Button(90)]
    private void SetDataCSV()
    {
        ReadTSVFile();
    }

    [ShowIf("Toggle")]
    [GUIColor(0,1,0)]
    [Button(90)]
    private void DONE()
    {
        
    }

    public void SetVisitorsName()
    {
        //Load Text asset
        TextAsset TSVText = Resources.Load<TextAsset>("TextAssets/NameData");

        //Add each line of the tab in a list 'lines'
        lines = TSVText.text.Split(new char[] { '\n' });

        nameScript.nameList.Clear();

        //For each line...
        for (int i = 1; i < lines.Length; i++)
        {
            Debug.Log("Read Lines => Visitor Name");

            nameScript.nameList.Add(lines[i]);
        }

        Debug.Log("Add " + lines.Length + " Visitor Name");
    }

    public void SetPathName()
    {
        //Load Text asset
        TextAsset TSVText = Resources.Load<TextAsset>("TextAssets/PathNameData");

        //Add each line of the tab in a list 'lines'
        lines = TSVText.text.Split(new char[] { '\n' });

        pathNameScript.nameList.Clear();

        //For each line...
        for (int i = 1; i < lines.Length; i++)
        {
            Debug.Log("Read Lines => Path Name");

            pathNameScript.nameList.Add(lines[i]);
        }

        Debug.Log("Add " + lines.Length + " Path Name");
    }
}
