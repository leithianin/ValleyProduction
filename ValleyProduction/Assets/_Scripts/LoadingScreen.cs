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
        TimerManager.CreateRealTimer(displayTime, () => OnShow?.Invoke());
    }
}
