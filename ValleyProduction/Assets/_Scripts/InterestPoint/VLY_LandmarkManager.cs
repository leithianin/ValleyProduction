using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VLY_LandmarkManager : VLY_Singleton<VLY_LandmarkManager>
{
    private List<LandmarkType> validLandmarkWithPath = new List<LandmarkType>();

    #region Actions
    public static Action<LandmarkType> OnLandmarkHasValidPath;
    public static Action<LandmarkType, VisitorBehavior> OnVisitorInteractWithLandmark;
    #endregion

    public List<LandmarkType> GetValidLandmark => validLandmarkWithPath;

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

    private void OnDestroy()
    {
        OnLandmarkHasValidPath = null;
    }
}
