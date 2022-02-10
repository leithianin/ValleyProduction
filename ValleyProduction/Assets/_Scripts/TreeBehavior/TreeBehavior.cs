using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TreeBehavior : MonoBehaviour
{
    [SerializeField] private int healthyScore;

    public GameObject OnSetMesh;
    public GameObject OnUnsetMesh;

    [SerializeField] private UnityEvent OnSet;
    [SerializeField] private UnityEvent OnUnset;

    [SerializeField] private UnityEvent<float> OnChangeScore;

    private bool isSet = false;

    public bool IsSet => isSet;

    private void Start()
    {
        OnChangeScore?.Invoke(healthyScore);
    }

    public void SetTree()
    {
        isSet = true;
        OnChangeScore?.Invoke(healthyScore);
        OnUnsetMesh.SetActive(false);
        OnSetMesh.SetActive(true);
        OnSet?.Invoke();
    }

    public void UnsetTree()
    {
        isSet = false;
        OnChangeScore?.Invoke(0);
        OnSetMesh.SetActive(false);
        OnUnsetMesh.SetActive(true);
        OnUnset?.Invoke();
    }
}
