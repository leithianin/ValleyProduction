using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CinematicShotsAssetPostProcessor : AssetPostprocessor
{
    public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        if (Scan("Assets/_Scripts/Camera/Data", importedAssets))
        {
            for (int i = 0; i < importedAssets.Length; i++)
            {
                Debug.Log(importedAssets[i]);
            }
            CinematicShotDataBase.AddShot(importedAssets[0]);
            CinematicShotDataBase database = (CinematicShotDataBase)AssetDatabase.LoadAssetAtPath("Assets/_Scripts/Camera/Data/CameraDataBase.asset", typeof(CinematicShotDataBase));

            if (importedAssets[0].Contains(".asset") && !importedAssets[0].Contains("DataBase"))
            {
                database.shotsDataBase.Add((CameraData)AssetDatabase.LoadAssetAtPath(importedAssets[0], typeof(CameraData)));
            }
        }

        if (Scan("Assets/_Scripts/Camera/Data", deletedAssets))
        {
            for (int i = 0; i < deletedAssets.Length; i++)
            {
                Debug.Log(deletedAssets[i]);
            }
            CinematicShotDataBase database = (CinematicShotDataBase)AssetDatabase.LoadAssetAtPath("Assets/_Scripts/Camera/Data/CameraDataBase.asset", typeof(CinematicShotDataBase));

            if (deletedAssets[0].Contains(".asset") && !deletedAssets[0].Contains("DataBase"))
            {
                for (int i = 0; i < database.shotsDataBase.Count; i++)
                {
                    Debug.Log(database.shotsDataBase.Count);

                    if (database.shotsDataBase[i] == null)
                    {
                        Debug.Log("Removed at index: " + i);
                        database.shotsDataBase.RemoveAt(i);
                    }
                }
            }
        }
    }

    public static bool Scan(string target, string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        return Scan(target, importedAssets) || Scan(target, deletedAssets) || Scan(target, movedAssets) || Scan(target, movedFromAssetPaths);
    }

    private static bool Scan(string traget, string[] arr)
    {
        foreach (var str in arr)
        {
            if (str.Contains(traget))
            {
                return true;
            }
        }
        return false;
    }
}
