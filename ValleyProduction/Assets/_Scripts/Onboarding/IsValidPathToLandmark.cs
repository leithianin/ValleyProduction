using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IsValidPathToLandmark : MonoBehaviour
{
    [SerializeField] private LandmarkType landmark;

    public UnityEvent OnValid;
    public UnityEvent OnUnvalid;

    private void Update()
    {
        if(VLY_LandmarkManager.GetValidLandmark.Contains(landmark))
        {
            OnValid?.Invoke();
        }
        else
        {
            OnUnvalid?.Invoke();
        }
    }
}
