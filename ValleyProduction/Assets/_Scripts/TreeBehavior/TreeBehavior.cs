using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TreeBehavior : MonoBehaviour
{
    [SerializeField] private int healthyScore;

    [HideInInspector] public int currentScore = 0;

    public GameObject OnSetMesh;
    public GameObject OnUnsetMesh;

    [SerializeField] private UnityEvent OnSet;
    [SerializeField] private UnityEvent OnUnset;

    private bool isSet = false;

    public bool IsSet => isSet;

    private void Start()
    {
        currentScore = healthyScore;
    }

    public void SetTree()
    {
        isSet = true;
        currentScore = healthyScore;
        OnUnsetMesh.SetActive(false);
        OnSetMesh.SetActive(true);
        OnSet?.Invoke();
    }

    public void UnsetTree()
    {
        isSet = false;
        currentScore = 0;
        OnSetMesh.SetActive(false);
        OnUnsetMesh.SetActive(true);
        OnUnset?.Invoke();
    }
}
