using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ReadNameDataTSV : MonoBehaviour
{
    string[] lines;         //List of all tab's lines
    string[] blocks;        //List of each line's columns

    /*public void Start()
    {
        ReadTSVFile();
    }*/

    public void ReadTSVFile()
    {
        SetVisitorsName();
        SetPathName();
    }

    public void SetVisitorsName()
    {
        //Load Text asset
        TextAsset TSVText = Resources.Load<TextAsset>("TextAssets/NameData");

        //Add each line of the tab in a list 'lines'
        lines = TSVText.text.Split(new char[] { '\n' });

        GeneratorManager.GetNameScript.nameList.Clear();

        //For each line...
        for (int i = 1; i < lines.Length; i++)
        {
            Debug.Log("Read Lines => Visitor Name");

            GeneratorManager.GetNameScript.nameList.Add(lines[i]);
        }

        Debug.Log("Add " + lines.Length + " Visitor Name");
    }

    public void SetPathName()
    {
        //Load Text asset
        TextAsset TSVText = Resources.Load<TextAsset>("TextAssets/PathNameData");

        //Add each line of the tab in a list 'lines'
        lines = TSVText.text.Split(new char[] { '\n' });

        GeneratorManager.GetPathNameScript.nameList.Clear();

        //For each line...
        for (int i = 1; i < lines.Length; i++)
        {
            Debug.Log("Read Lines => Path Name");

            GeneratorManager.GetPathNameScript.nameList.Add(lines[i]);
        }

        Debug.Log("Add " + lines.Length + " Path Name");
    }
}
