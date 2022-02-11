using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkDisplayer : MonoBehaviour
{
    [SerializeField] private List<MeshRenderer> meshes = new List<MeshRenderer>();
    [SerializeField] private BoxCollider collider;
    [SerializeField] private bool visible = true;

    public void ResetMeshes()
    {
        meshes = new List<MeshRenderer>();
    }

    public void AddMeshes(MeshRenderer toAdd)
    {
        if (!meshes.Contains(toAdd))
        {
            meshes.Add(toAdd);
        }
    }

    private void OnVisible()
    {
        for(int i= 0; i < meshes.Count; i++)
        {
            meshes[i].enabled = true;
        }
    }

    private void OnInvisible()
    {
        for (int i = 0; i < meshes.Count; i++)
        {
            meshes[i].enabled = false;
        }
    }

    private void Update()
    {
        if (!visible && PlayerInputManager.GetCamera.IsObjectVisible(collider.bounds))
        {
            visible = true;
            OnVisible();
        }
        else if (visible && !PlayerInputManager.GetCamera.IsObjectVisible(collider.bounds))
        {
            visible = false;
            OnInvisible();
        }
    }
}
