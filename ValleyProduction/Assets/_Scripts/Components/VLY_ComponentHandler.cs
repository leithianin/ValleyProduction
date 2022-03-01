using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VLY_ComponentHandler : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRnd;
    [SerializeField] private List<VLY_Component> components;

    public MeshRenderer MeshRnd => meshRnd;

    public void GetComponentOfType<T>(ref T wantedComponent) where T : VLY_Component
    {
        for(int i = 0; i < components.Count; i++)
        {
            if(components[i] as T != null)
            {
                wantedComponent = components[i] as T;
                break;
            }
        }
    }
}
