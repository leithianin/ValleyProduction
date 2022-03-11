using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HeatMapMaskRenderer : MonoBehaviour
{
    private static List<Area> chunks;

    public static void RegisterChunks(Area chunk) { chunks.Add(chunk); }

    //Properties
    [SerializeField] private ComputeShader computeShader = null;

    [Range(64, 4096)] [SerializeField] private int TextureSize = 2048;
    [SerializeField] private float MapSize = 0;

    [SerializeField] private float BlendDistance = 4f;

    public Color MaskColor0;
    public Color MaskColor1;
    public Color MaskColor2;
    public Color MaskColor3;

    public Texture2D NoiseTexture;
    [Range(0f, 5f)] public float NoiseDetail = 4f;

    private RenderTexture maskTexture;

    //Caching shader properties
    private static readonly int textureSizeId = Shader.PropertyToID("_TextureSize");
    private static readonly int chunkCountId = Shader.PropertyToID("_ChunkCount");
    private static readonly int mapSizeId = Shader.PropertyToID("_MapSize");
    private static readonly int blendId = Shader.PropertyToID("_Blend");

    private static readonly int color0Id = Shader.PropertyToID("_Color0");
    private static readonly int color1Id = Shader.PropertyToID("_Color1");
    private static readonly int color2Id = Shader.PropertyToID("_Color2");
    private static readonly int color3Id = Shader.PropertyToID("_Color3");

    private static readonly int noiseTexId = Shader.PropertyToID("_NoiseTex");
    private static readonly int noiseDetailId = Shader.PropertyToID("_NoiseDetail");

    private static readonly int maskTextureId = Shader.PropertyToID("_Mask");

    private static readonly int chunkBufferId = Shader.PropertyToID("_ChunkBuffer");

    //Chunk info filled in a buffer
    private struct ChunkBufferElement
    {
        public float PositionX;
        public float PositionY;
        public float Noise;
    }

    private List<ChunkBufferElement> bufferElements;
    private ComputeBuffer buffer = null;

    private void Awake()
    {
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        maskTexture = new RenderTexture(TextureSize, TextureSize, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear)
#else
        maskTexture = new RenderTexture(TextureSize, TextureSize, 0, RenderTextureFormat.ARGB32)
#endif
        {
            enableRandomWrite = true
        };
        maskTexture.Create();

        computeShader.SetInt(textureSizeId, TextureSize);
        computeShader.SetTexture(0, maskTextureId, maskTexture);

        computeShader.SetFloat(blendId, BlendDistance);

        computeShader.SetVector(color0Id, MaskColor0);
        computeShader.SetVector(color1Id, MaskColor1);
        computeShader.SetVector(color2Id, MaskColor2);
        computeShader.SetVector(color3Id, MaskColor3);

        computeShader.SetTexture(0, noiseTexId, NoiseTexture);
        computeShader.SetFloat(noiseDetailId, NoiseDetail);

        Shader.SetGlobalTexture(maskTextureId, maskTexture);
        Shader.SetGlobalFloat(mapSizeId, MapSize);

        bufferElements = new List<ChunkBufferElement>();
    }

    private void OnDestroy()
    {
        buffer?.Dispose();

        if (maskTexture != null) DestroyImmediate(maskTexture);
    }

    private void Update()
    {
        bufferElements.Clear();

        foreach(Area chunk in chunks)
        {
            ChunkBufferElement element = new ChunkBufferElement
            {
                PositionX = chunk.GetWorldPosition.x,
                PositionY = chunk.GetWorldPosition.z,
                Noise = chunk.GetData<AU_MakeSound>()[0].Score,
            };
            bufferElements.Add(element);
        }

        buffer?.Release();
        buffer = new ComputeBuffer(bufferElements.Count * 4, sizeof(float));

        buffer.SetData(bufferElements);
        computeShader.SetBuffer(0, chunkBufferId, buffer);

        computeShader.SetInt(chunkCountId, bufferElements.Count);

        computeShader.Dispatch(0, Mathf.CeilToInt(TextureSize / 8.0f), Mathf.CeilToInt(TextureSize / 8.0f), 1);
    }
}
