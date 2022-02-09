using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OB_FirstMarker : OnBoarding
{
    public UnityEvent OnFirstMarker;

    protected override void OnEnd()
    {
        
    }

    protected override void OnPlay()
    {
        PathManager.isOnSpawn += OnPlaceSpawn;
    }

    public void OnPlaceSpawn(bool isOnPlaza)
    {
        PathManager.isOnSpawn -= OnPlaceSpawn;
        OnFirstMarker?.Invoke();
    }
}
