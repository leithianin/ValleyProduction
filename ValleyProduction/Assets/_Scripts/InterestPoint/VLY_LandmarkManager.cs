using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VLY_LandmarkManager : VLY_Singleton<VLY_LandmarkManager>
{
    private List<LandmarkType> validLandmarkWithPath = new List<LandmarkType>();

    private List<CPN_IsLandmark> landmarkOnMap = new List<CPN_IsLandmark>();

    #region Actions
    public static Action<LandmarkType> OnLandmarkHasValidPath;
    public static Action<LandmarkType, VisitorBehavior> OnVisitorInteractWithLandmark;
    #endregion

    public List<LandmarkType> GetValidLandmark => validLandmarkWithPath;

    public static List<CPN_IsLandmark> AllLandmarks => instance.landmarkOnMap;

    private void Start()
    {
        landmarkOnMap = new List<CPN_IsLandmark>(FindObjectsOfType<CPN_IsLandmark>());
    }

    public static void AddValidLandmark(LandmarkType nLandmark)
    {
        if(!instance.validLandmarkWithPath.Contains(nLandmark))
        {
            instance.validLandmarkWithPath.Add(nLandmark);
            OnLandmarkHasValidPath?.Invoke(nLandmark);
        }
    }

    public static void RemoveValidLandmark(LandmarkType nLandmark)
    {
        if (instance.validLandmarkWithPath.Contains(nLandmark))
        {
            instance.validLandmarkWithPath.Remove(nLandmark);
        }
    }

    public static void OnLandmarkInteraction(LandmarkType landmark, VisitorBehavior interactor)
    {
        OnVisitorInteractWithLandmark?.Invoke(landmark, interactor);
    }
    
    public static List<CPN_IsLandmark> GetLandmarkOfType(LandmarkType type)
    {
        List<CPN_IsLandmark> toReturn = new List<CPN_IsLandmark>();

        for(int i = 0; i < AllLandmarks.Count; i++)
        {
            if(AllLandmarks[i].Type == type)
            {
                toReturn.Add(AllLandmarks[i]);
            }
        }

        return toReturn;
    }

    private void OnDestroy()
    {
        OnLandmarkHasValidPath = null;
        landmarkOnMap = new List<CPN_IsLandmark>();
    }
}
