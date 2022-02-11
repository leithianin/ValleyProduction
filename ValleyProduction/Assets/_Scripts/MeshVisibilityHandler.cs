using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class MeshVisibilityHandler : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.WithAll<MeshVisibilityComponent>().ForEach((ref MeshVisibilityComponent meshVisibility) =>
        {
            MeshRenderer mesh = meshVisibility.mesh;
            if (!mesh.enabled && PlayerInputManager.GetCamera.IsObjectVisible(mesh.bounds))
            {
                mesh.enabled = true;
            }
            else if (mesh.enabled && !PlayerInputManager.GetCamera.IsObjectVisible(mesh.bounds))
            {
                mesh.enabled = false;
            }
        }).Schedule();
    }
}
