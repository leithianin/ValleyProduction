using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraDataBase", menuName = "CameraDataBase", order = 1)]
public class CinematicShotDataBase : ScriptableObject
{
    public  List<CameraData> shotsDataBase = new List<CameraData>();

    public static void AddShot(string dataPath)
    {

    }

}
