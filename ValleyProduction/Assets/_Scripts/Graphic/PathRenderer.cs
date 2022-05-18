using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathRenderer : VLY_Singleton<PathRenderer>
{
    #region Paths properties
    [Header("Path Data"), SerializeField] private List<PathFragmentData> pathFragments = new List<PathFragmentData>();

    private static List<PathFragmentData> PathFragments => instance.pathFragments;

    public static void RegisterPathFragment(PathFragmentData frag) 
    { 
        if(!instance.enabled)
        {
            instance.enabled = true;
        }
        instance.pathFragments.Add(frag); 
    }
    #endregion

    #region Compute properties
    [Header("Compute Properties"), SerializeField] private ComputeShader compute;

    [Range(64, 1024)] [SerializeField] public static int TextureSize = 1024;
    [SerializeField] private float mapSize = 0;
    public float MapSize => mapSize;

    public RenderTexture pathTexture;

    [Space] public float pathThickness;
    #endregion

    #region Shader properties
    [Header("Shader Properties")] public Material terrainMat;
    [Space] public Texture noiseTex;
    public float noiseDetail;
    public float noisePower;
    #endregion

    #region Shader properties cache
    private static readonly int textureSizeId = Shader.PropertyToID("_TextureSize");
    private static readonly int pathpointCountId = Shader.PropertyToID("_PathPointCount");
    private static readonly int mapSizeId = Shader.PropertyToID("_MapSize");

    private static readonly int pathThicknessId = Shader.PropertyToID("_PathThickness");

    private static readonly int noiseTexId = Shader.PropertyToID("_NoiseTex");
    private static readonly int noiseDetailId = Shader.PropertyToID("_NoiseDetail");
    private static readonly int noisePowerId = Shader.PropertyToID("_NoisePower");

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

    protected override void OnAwake()
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

        terrainMat.SetTexture("PATHS", pathTexture);
        terrainMat.SetFloat("_MapSize", mapSize);

        bufferElements = new List<PathpointBufferElement>();
    }

    private void Start()
    {
        enabled = false;
    }

    private void OnDestroy()
    {
        pathpointBuffer?.Dispose();

        if (pathTexture != null)
            DestroyImmediate(pathTexture);
    }

    private void LateUpdate()
    {
        bufferElements = new List<PathpointBufferElement>();

        if (PathFragments != null)
        {

            foreach (PathFragmentData frag in PathFragments)
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

            if (bufferElements.Count > 0)
            {
                pathpointBuffer = new ComputeBuffer(bufferElements.Count * 4, sizeof(float));

                compute.SetBuffer(0, pathpointBufferId, pathpointBuffer);

                pathpointBuffer.SetData(bufferElements);
            }


            compute.SetTexture(0, noiseTexId, noiseTex);

            compute.SetInt(pathpointCountId, bufferElements.Count);
            compute.SetFloat(pathThicknessId, pathThickness);
            compute.SetFloat(noiseDetailId, noiseDetail);
            compute.SetFloat(noisePowerId, noisePower);

            compute.Dispatch(0, Mathf.CeilToInt(TextureSize / 8.0f), Mathf.CeilToInt(TextureSize / 8.0f), 1);

        }

        enabled = false;
    }

    public static void RemoveFragment(PathFragmentData toRemove)
    {
        if (!instance.enabled)
        {
            instance.enabled = true;
        }
        instance.pathFragments.Remove(toRemove);
    }
}
