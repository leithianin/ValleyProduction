using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TreeBehavior : MonoBehaviour
{
    [SerializeField] private int healthyScore;

    [HideInInspector] public int currentScore = 0;

    public MeshFilter meshComponent;

    public Mesh OnSetMesh;
    public Mesh OnUnsetMesh;

    [SerializeField] private UnityEvent OnSet;
    [SerializeField] private UnityEvent OnUnset;

    private bool isSet = false;

    public bool IsSet => isSet;

    public void SetTree()
    {
        isSet = true;
        currentScore = healthyScore;
        meshComponent.mesh = OnSetMesh;
        OnSet?.Invoke();
    }

    public void UnsetTree()
    {
        isSet = false;
        currentScore = 0;
        meshComponent.mesh = OnUnsetMesh;
        OnUnset?.Invoke();
    }
}
