using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicalChunkManager : MonoBehaviour
{
    /// Dimensions de la map
    [SerializeField] private Vector2 worldDimension;
    /// Taille de la grille
    [SerializeField] private float chunkLength;
    [SerializeField] private bool showGizmos;
    [SerializeField] private ChunkGraphical chunkPrefab;
    [SerializeField] private Transform chunkHandler;

    /// Dimensions de la grille
    private static Vector2Int gridDimension;

    [SerializeField] private List<ChunkGraphical> chunks;

    private Vector2 CenterPosition => new Vector2(transform.position.x, transform.position.z);

    /// <summary>
    /// Crée la grille, et assigne tous les AreaDisplay aux zones.
    /// </summary>
    [ContextMenu("Set Chunks")]
    private void SetChunks()
    {
        while(chunkHandler.childCount > 0)
        {
            DestroyImmediate(chunkHandler.GetChild(0));
        }

        chunks = new List<ChunkGraphical>();

        gridDimension = new Vector2Int(Mathf.RoundToInt(worldDimension.x / chunkLength), Mathf.RoundToInt(worldDimension.y / chunkLength));

        for (int i = 0; i < gridDimension.x; i++)
        {
            for (int j = 0; j < gridDimension.y; j++)
            {
                ChunkGraphical newChunk = Instantiate(chunkPrefab, chunkHandler);
                newChunk.SetChunk(chunkLength, new Vector3(chunkLength / 2f + chunkLength * i, 0, chunkLength / 2f + chunkLength * j) + new Vector3(GetWorldPositionOffset().x, 0, GetWorldPositionOffset().y));
                chunks.Add(newChunk);
            }
        }
    }

    /*private void Start()
    {
        for(int i = 0; i < chunkHandler.childCount; i++)
        {
            chunks.Add(chunkHandler.GetChild(0).GetComponent<ChunkGraphical>());
        }
    }*/

    private Vector2 GetWorldPositionOffset()
    {
        return CenterPosition - (worldDimension / 2f);
    }

    private void Update()
    {
        for (int i = 0; i < chunks.Count; i++)
        {
            if (PlayerInputManager.GetCamera.IsObjectVisible(chunks[i].collider.bounds))
            {
                chunks[i].SetVisible();
            }
            else if (!PlayerInputManager.GetCamera.IsObjectVisible(chunks[i].collider.bounds))
            {
                chunks[i].SetInvisible();
            }
        }
    }
}
