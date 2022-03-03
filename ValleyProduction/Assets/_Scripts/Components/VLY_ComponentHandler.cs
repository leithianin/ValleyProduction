using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VLY_ComponentHandler : MonoBehaviour
{
    [SerializeField] private List<VLY_Component> components;

    public T GetComponentOfType<T>() where T : VLY_Component
    {
        for(int i = 0; i < components.Count; i++)
        {
            if(components[i] != null && components[i] is T)
            {
                return components[i] as T;
            }
        }
        return null;
    }
}
