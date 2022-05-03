using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathRenderer : MonoBehaviour
{
    [SerializeField] private ComputeShader compute;

    [Range(64, 1024)] [SerializeField] public static int TextureSize = 256;
    [SerializeField] private float mapSize = 0;
    public float MapSize => mapSize;

    public RenderTexture pathTexture;

    #region Shader properties cache
    private static readonly int textureSizeId = Shader.PropertyToID("_TextureSize");
    private static readonly int mapSizeId = Shader.PropertyToID("_MapSize");

    private static readonly int pathTextureId = Shader.PropertyToID("_PathTex");
    #endregion

    private void Awake()
    {
        
    }

    private void Start()
    {
        
    }
}
