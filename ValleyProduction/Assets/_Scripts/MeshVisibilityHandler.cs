using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class MeshVisibilityHandler : MonoBehaviour
{
    [SerializeField] public MeshRenderer mesh;

    /*private void Update()
    {
        if(!mesh.enabled && PlayerInputManager.GetCamera.IsObjectVisible(mesh))
        {
            mesh.enabled = true;
        }
        else if (mesh.enabled && !PlayerInputManager.GetCamera.IsObjectVisible(mesh))
        {
            mesh.enabled = false;
        }
    }*/
}
