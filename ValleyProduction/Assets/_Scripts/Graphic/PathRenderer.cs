using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathRenderer : VLY_Singleton<PathRenderer>
{
    #region Paths properties
    private static List<PathFragmentData> pathFragments = new List<PathFragmentData>();
    public static void RegisterPathFragment(PathFragmentData frag) 
    { 
        pathFragments.Add(frag); 
    }
    #endregion

    #region Properties
    [SerializeField] private ComputeShader compute;

    [Range(64, 1024)] [SerializeField] public static int TextureSize = 256;
    [SerializeField] private float mapSize = 0;
    public float MapSize => mapSize;

    public RenderTexture pathTexture;
    #endregion

    #region Shader properties cache
    private static readonly int textureSizeId = Shader.PropertyToID("_TextureSize");
    private static readonly int pathpointCountId = Shader.PropertyToID("_PathPointCount");
    private static readonly int mapSizeId = Shader.PropertyToID("_MapSize");

    private static readonly int pathTextureId = Shader.PropertyToID("_PathTex");

    private static readonly int pathpointBufferId = Shader.PropertyToID("_PathPointBuffer");
    #endregion

    #region Path point buffer
    private struct PathpointBufferElement
    {
        public float StartPositionX;
        public float StartPositionY;
        public float EndPositionX;
        public float EndPositionY;
    }

    private List<PathpointBufferElement> bufferElements;
    private ComputeBuffer pathpointBuffer = null;
    #endregion

    private void Awake()
    {
        #region Create texture
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        pathTexture = new RenderTexture(TextureSize, TextureSize, 0, RenderTextureFormat.ARG32, RenderTextureReadWrite.Linear)
#else
        pathTexture = new RenderTexture(TextureSize, TextureSize, 0, RenderTextureFormat.ARGBFloat)
#endif
        {
            enableRandomWrite = true
        };
        pathTexture.name = "T_PathMask";
        pathTexture.Create();
        #endregion

        compute.SetInt(textureSizeId, TextureSize);
        compute.SetTexture(0, pathTextureId, pathTexture);

        Shader.SetGlobalTexture(pathTextureId, pathTexture);
        Shader.SetGlobalFloat(mapSizeId, mapSize);

        bufferElements = new List<PathpointBufferElement>();
    }

    private void OnDestroy()
    {
        pathpointBuffer?.Dispose();

        if (pathTexture != null)
            DestroyImmediate(pathTexture);
    }

    private void Update()
    {
        bufferElements.Clear();

        if(pathFragments != null)
        {
            Debug.Log(pathFragments.Count);
            if (pathFragments.Count > 0)
            {
                foreach (PathFragmentData frag in pathFragments)
                {
                    for (int i = 0; i < frag.path.Count - 1; i++)
                    {
                        PathpointBufferElement element = new PathpointBufferElement
                        {
                            StartPositionX = frag.path[i].x,
                            StartPositionY = frag.path[i].z,
                            EndPositionX = frag.path[i + 1].x,
                            EndPositionY = frag.path[i + 1].z
                        };
                        bufferElements.Add(element);
                    }
                }

                pathpointBuffer = new ComputeBuffer(bufferElements.Count * 4, sizeof(float));

                compute.SetBuffer(0, pathpointBufferId, pathpointBuffer);

                pathpointBuffer.SetData(bufferElements);

                compute.SetInt(pathpointCountId, bufferElements.Count);

                Debug.Log(compute);
                compute.Dispatch(0, Mathf.CeilToInt(TextureSize / 8.0f), Mathf.CeilToInt(TextureSize / 8.0f), 1);
            }

            //compute.Dispatch(0, Mathf.CeilToInt(TextureSize / 8.0f), Mathf.CeilToInt(TextureSize / 8.0f), 1);
        }
        
    }

    public static void RemoveFragment(PathFragmentData toRemove)
    {

    }
}
