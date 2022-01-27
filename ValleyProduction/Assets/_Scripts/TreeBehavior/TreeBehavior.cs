using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TreeBehavior : MonoBehaviour
{
    public MeshFilter meshComponent;

    public Mesh OnSetMesh;
    public Mesh OnUnsetMesh;

    [SerializeField] private UnityEvent OnSet;
    [SerializeField] private UnityEvent OnUnset;

    public void SetTree()
    {
        meshComponent.mesh = OnSetMesh;
        OnSet?.Invoke();
    }

    public void UnsetTree()
    {
        meshComponent.mesh = OnUnsetMesh;
        OnUnset?.Invoke();
    }
}
