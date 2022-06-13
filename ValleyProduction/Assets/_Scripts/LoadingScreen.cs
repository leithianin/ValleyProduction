using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private float displayTime;

    [SerializeField] private UnityEvent OnShow;
    [SerializeField] private UnityEvent OnHide;

    private void Start()
    {
        if (displayTime >= 0)
        {
            TimerManager.CreateRealTimer(displayTime, HideLoadingScreen);
        }
    }

    public void HideLoadingScreen()
    {
        OnHide?.Invoke();
    }

    public void ShowLoadingScreen()
    {
        OnShow?.Invoke();
    }
}
