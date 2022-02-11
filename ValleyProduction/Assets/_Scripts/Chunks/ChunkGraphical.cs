using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkGraphical : MonoBehaviour
{
    [SerializeField] private List<MeshVisibilityHandler> meshHandler;
    public BoxCollider collider;

    public bool isVisible = true;

    public void SetChunk(float areaLength, Vector3 position)
    {
        collider.size = new Vector3(areaLength, areaLength, areaLength);
        transform.position = position;

        meshHandler = new List<MeshVisibilityHandler>();

        Collider[] colliderInArea = Physics.OverlapBox(transform.position, Vector3.one * areaLength / 2f);

        for (int i = 0; i < colliderInArea.Length; i++)
        {
            if (colliderInArea[i].GetComponent<MeshVisibilityHandler>() != null)
            {
                meshHandler.Add(colliderInArea[i].GetComponent<MeshVisibilityHandler>());
            }
        }
    }

    public void SetVisible()
    {
        if (!isVisible)
        {
            isVisible = true;
            for (int i = 0; i < meshHandler.Count; i++)
            {
                meshHandler[i].mesh.enabled = true;
            }
        }
    }

    public void SetInvisible()
    {
        if (isVisible)
        {
            isVisible = false;
            for (int i = 0; i < meshHandler.Count; i++)
            {
                meshHandler[i].mesh.enabled = false;
            }
        }
    }
}
