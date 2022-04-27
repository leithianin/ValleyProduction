using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VLY_EcosystemManager : VLY_Singleton<VLY_EcosystemManager>
{
    [SerializeField] private MaskRenderer heatmapHandler;

    public static int GetScoreAtPosition(Vector2 position, EcosystemDataType scoreType)
    {
        return instance.heatmapHandler.GetScoreAtPosition(position, scoreType);
    }

    public static void AddAgent(EcosystemAgent toAdd)
    {
        instance.heatmapHandler.AddAgent(toAdd);
    }

    public static void RemoveAgent(EcosystemAgent toRemove)
    {
        instance.heatmapHandler.RemoveAgent(toRemove);
    }

    public Transform transformTest;

    [ContextMenu("Test get Noise score of transform")]
    public void Test()
    {
        Debug.Log(GetScoreAtPosition(new Vector2(transformTest.position.x, transformTest.position.z), EcosystemDataType.Noise));
    }
}
