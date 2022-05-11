using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfrastructureTransparency : MonoBehaviour
{
    [SerializeField] private List<Renderer> renderers;
    [SerializeField] private string shader_variable_name;

    public void SetTransparency(bool isTransparent)
    {
        foreach(Renderer r in renderers)
        {
            r.material.SetInt(shader_variable_name, isTransparent ? 1 : 0);
        }
    }
}
