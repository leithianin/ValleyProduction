using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MaskRenderer : MonoBehaviour
{
    #region Entities properties
    [Obsolete]
    private static List<Entity> entities;
    [Obsolete]
    public static void RegisterEntity(Entity entity) { entities.Add(entity); }

    [SerializeField] private List<EcosystemAgent> ecosystemAgents = new List<EcosystemAgent>();
    #endregion

    #region Renderer properties
    [SerializeField] private ComputeShader compute = null;

    [Range(64, 1024)] [SerializeField] public static int TextureSize = 256;
    [SerializeField] private float mapSize = 0;
    public float MapSize => mapSize;

    [SerializeField] private float BlendDistance = 4.0f;

    public Gradient NoiseGradient;
    public Gradient PollutionGradient;
    public Gradient FaunaGradient;
    public Gradient FloraGradient;

    public RenderTexture noiseTexture;
    public RenderTexture pollutionTexture;
    public RenderTexture faunaTexture;
    public RenderTexture floraTexture;
    #endregion

    #region Shader properties cache
    private static readonly int textureSizeId = Shader.PropertyToID("_TextureSize");
    private static readonly int entityCountId = Shader.PropertyToID("_EntityCount");
    private static readonly int mapSizeId = Shader.PropertyToID("_MapSize");
    private static readonly int blendId = Shader.PropertyToID("_Blend");

    #region Gradients
    private static readonly int noiseColor0Id = Shader.PropertyToID("_NoiseColor0");
    private static readonly int noiseColor1Id = Shader.PropertyToID("_NoiseColor1");
    private static readonly int noiseColor2Id = Shader.PropertyToID("_NoiseColor2");
    private static readonly int noiseColor3Id = Shader.PropertyToID("_NoiseColor3");
    private static readonly int noiseColor4Id = Shader.PropertyToID("_NoiseColor4");
    private static readonly int noiseColor5Id = Shader.PropertyToID("_NoiseColor5");
    private static readonly int noiseColor6Id = Shader.PropertyToID("_NoiseColor6");
    private static readonly int noiseColor7Id = Shader.PropertyToID("_NoiseColor7");
    private static readonly int noiseColor8Id = Shader.PropertyToID("_NoiseColor8");
    private static readonly int noiseColor9Id = Shader.PropertyToID("_NoiseColor9");
    private static readonly int noiseColor10Id = Shader.PropertyToID("_NoiseColor10");

    private static readonly int pollutionColor0Id = Shader.PropertyToID("_PollutionColor0");
    private static readonly int pollutionColor1Id = Shader.PropertyToID("_PollutionColor1");
    private static readonly int pollutionColor2Id = Shader.PropertyToID("_PollutionColor2");
    private static readonly int pollutionColor3Id = Shader.PropertyToID("_PollutionColor3");
    private static readonly int pollutionColor4Id = Shader.PropertyToID("_PollutionColor4");
    private static readonly int pollutionColor5Id = Shader.PropertyToID("_PollutionColor5");
    private static readonly int pollutionColor6Id = Shader.PropertyToID("_PollutionColor6");
    private static readonly int pollutionColor7Id = Shader.PropertyToID("_PollutionColor7");
    private static readonly int pollutionColor8Id = Shader.PropertyToID("_PollutionColor8");
    private static readonly int pollutionColor9Id = Shader.PropertyToID("_PollutionColor9");
    private static readonly int pollutionColor10Id = Shader.PropertyToID("_PollutionColor10");

    private static readonly int faunaColor0Id = Shader.PropertyToID("_FaunaColor0");
    private static readonly int faunaColor1Id = Shader.PropertyToID("_FaunaColor1");
    private static readonly int faunaColor2Id = Shader.PropertyToID("_FaunaColor2");
    private static readonly int faunaColor3Id = Shader.PropertyToID("_FaunaColor3");
    private static readonly int faunaColor4Id = Shader.PropertyToID("_FaunaColor4");
    private static readonly int faunaColor5Id = Shader.PropertyToID("_FaunaColor5");
    private static readonly int faunaColor6Id = Shader.PropertyToID("_FaunaColor6");
    private static readonly int faunaColor7Id = Shader.PropertyToID("_FaunaColor7");
    private static readonly int faunaColor8Id = Shader.PropertyToID("_FaunaColor8");
    private static readonly int faunaColor9Id = Shader.PropertyToID("_FaunaColor9");
    private static readonly int faunaColor10Id = Shader.PropertyToID("_FaunaColor10");

    private static readonly int floraColor0Id = Shader.PropertyToID("_FloraColor0");
    private static readonly int floraColor1Id = Shader.PropertyToID("_FloraColor1");
    private static readonly int floraColor2Id = Shader.PropertyToID("_FloraColor2");
    private static readonly int floraColor3Id = Shader.PropertyToID("_FloraColor3");
    private static readonly int floraColor4Id = Shader.PropertyToID("_FloraColor4");
    private static readonly int floraColor5Id = Shader.PropertyToID("_FloraColor5");
    private static readonly int floraColor6Id = Shader.PropertyToID("_FloraColor6");
    private static readonly int floraColor7Id = Shader.PropertyToID("_FloraColor7");
    private static readonly int floraColor8Id = Shader.PropertyToID("_FloraColor8");
    private static readonly int floraColor9Id = Shader.PropertyToID("_FloraColor9");
    private static readonly int floraColor10Id = Shader.PropertyToID("_FloraColor10");
    #endregion

    private static readonly int noiseTextureId = Shader.PropertyToID("_NoiseTex");
    private static readonly int pollutionTextureId = Shader.PropertyToID("_PollutionTex");
    private static readonly int faunaTextureId = Shader.PropertyToID("_FaunaTex");
    private static readonly int floraTextureId = Shader.PropertyToID("_FloraTex");

    private static readonly int entityBufferId = Shader.PropertyToID("_EntityBuffer");
    private static readonly int pixelBufferId = Shader.PropertyToID("_PixelBuffer");
    #endregion

    #region Entity data buffer
    private struct EntityBufferElement
    {
        public float PositionX;
        public float PositionY;
        public float Range;
        public float Noise;
        public float Pollution;
        public float Fauna;
        public float Flora;
    }

    private List<EntityBufferElement> bufferElements;
    private ComputeBuffer entityBuffer = null;


    [SerializeField] private int[] ecosystemGrids;

    private ComputeBuffer pixelsBuffer = null;
    #endregion

    public List<EcosystemAgent> Agents => ecosystemAgents;

    private void Awake()
    {
        entities = new List<Entity>();

        #region Create textures
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        noiseTexture = new RenderTexture(TextureSize, TextureSize, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear)
#else
        noiseTexture = new RenderTexture(TextureSize, TextureSize, 0, RenderTextureFormat.ARGBFloat)
#endif
        {
            enableRandomWrite = true
        };
        noiseTexture.name = "T_NoiseMap";
        noiseTexture.Create();

#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        pollutionTexture = new RenderTexture(TextureSize, TextureSize, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear)
#else
        pollutionTexture = new RenderTexture(TextureSize, TextureSize, 0, RenderTextureFormat.ARGBFloat)
#endif
        {
            enableRandomWrite = true
        };
        pollutionTexture.name = "T_PollutionMap";
        pollutionTexture.Create();

#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        faunaTexture = new RenderTexture(TextureSize, TextureSize, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear)
#else
        faunaTexture = new RenderTexture(TextureSize, TextureSize, 0, RenderTextureFormat.ARGBFloat)
#endif
        {
            enableRandomWrite = true
        };
        faunaTexture.name = "T_FaunaMap";
        faunaTexture.Create();

#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        floraTexture = new RenderTexture(TextureSize, TextureSize, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear)
#else
        floraTexture = new RenderTexture(TextureSize, TextureSize, 0, RenderTextureFormat.ARGBFloat)
#endif
        {
            enableRandomWrite = true
        };
        floraTexture.name = "T_FloraMap";
        floraTexture.Create();
        #endregion

        compute.SetInt(textureSizeId, TextureSize);
        compute.SetTexture(0, noiseTextureId, noiseTexture);
        compute.SetTexture(0, pollutionTextureId, pollutionTexture);
        compute.SetTexture(0, faunaTextureId, faunaTexture);
        compute.SetTexture(0, floraTextureId, floraTexture);

        compute.SetFloat(blendId, BlendDistance);

        #region Evaluate gradients
        compute.SetVector(noiseColor0Id, NoiseGradient.Evaluate(0));
        compute.SetVector(noiseColor1Id, NoiseGradient.Evaluate(0.1f));
        compute.SetVector(noiseColor2Id, NoiseGradient.Evaluate(0.2f));
        compute.SetVector(noiseColor3Id, NoiseGradient.Evaluate(0.3f));
        compute.SetVector(noiseColor4Id, NoiseGradient.Evaluate(0.4f));
        compute.SetVector(noiseColor5Id, NoiseGradient.Evaluate(0.5f));
        compute.SetVector(noiseColor6Id, NoiseGradient.Evaluate(0.6f));
        compute.SetVector(noiseColor7Id, NoiseGradient.Evaluate(0.7f));
        compute.SetVector(noiseColor8Id, NoiseGradient.Evaluate(0.8f));
        compute.SetVector(noiseColor9Id, NoiseGradient.Evaluate(0.9f));
        compute.SetVector(noiseColor10Id, NoiseGradient.Evaluate(1));

        compute.SetVector(pollutionColor0Id, PollutionGradient.Evaluate(0));
        compute.SetVector(pollutionColor1Id, PollutionGradient.Evaluate(0.1f));
        compute.SetVector(pollutionColor2Id, PollutionGradient.Evaluate(0.2f));
        compute.SetVector(pollutionColor3Id, PollutionGradient.Evaluate(0.3f));
        compute.SetVector(pollutionColor4Id, PollutionGradient.Evaluate(0.4f));
        compute.SetVector(pollutionColor5Id, PollutionGradient.Evaluate(0.5f));
        compute.SetVector(pollutionColor6Id, PollutionGradient.Evaluate(0.6f));
        compute.SetVector(pollutionColor7Id, PollutionGradient.Evaluate(0.7f));
        compute.SetVector(pollutionColor8Id, PollutionGradient.Evaluate(0.8f));
        compute.SetVector(pollutionColor9Id, PollutionGradient.Evaluate(0.9f));
        compute.SetVector(pollutionColor10Id, PollutionGradient.Evaluate(1));

        compute.SetVector(faunaColor0Id, FaunaGradient.Evaluate(0));
        compute.SetVector(faunaColor1Id, FaunaGradient.Evaluate(0.1f));
        compute.SetVector(faunaColor2Id, FaunaGradient.Evaluate(0.2f));
        compute.SetVector(faunaColor3Id, FaunaGradient.Evaluate(0.3f));
        compute.SetVector(faunaColor4Id, FaunaGradient.Evaluate(0.4f));
        compute.SetVector(faunaColor5Id, FaunaGradient.Evaluate(0.5f));
        compute.SetVector(faunaColor6Id, FaunaGradient.Evaluate(0.6f));
        compute.SetVector(faunaColor7Id, FaunaGradient.Evaluate(0.7f));
        compute.SetVector(faunaColor8Id, FaunaGradient.Evaluate(0.8f));
        compute.SetVector(faunaColor9Id, FaunaGradient.Evaluate(0.9f));
        compute.SetVector(faunaColor10Id, FaunaGradient.Evaluate(1));

        compute.SetVector(floraColor0Id, FloraGradient.Evaluate(0));
        compute.SetVector(floraColor1Id, FloraGradient.Evaluate(0.1f));
        compute.SetVector(floraColor2Id, FloraGradient.Evaluate(0.2f));
        compute.SetVector(floraColor3Id, FloraGradient.Evaluate(0.3f));
        compute.SetVector(floraColor4Id, FloraGradient.Evaluate(0.4f));
        compute.SetVector(floraColor5Id, FloraGradient.Evaluate(0.5f));
        compute.SetVector(floraColor6Id, FloraGradient.Evaluate(0.6f));
        compute.SetVector(floraColor7Id, FloraGradient.Evaluate(0.7f));
        compute.SetVector(floraColor8Id, FloraGradient.Evaluate(0.8f));
        compute.SetVector(floraColor9Id, FloraGradient.Evaluate(0.9f));
        compute.SetVector(floraColor10Id, FloraGradient.Evaluate(1));
        #endregion

        Shader.SetGlobalTexture(noiseTextureId, noiseTexture);
        Shader.SetGlobalTexture(pollutionTextureId, pollutionTexture);
        Shader.SetGlobalTexture(faunaTextureId, faunaTexture);
        Shader.SetGlobalTexture(floraTextureId, floraTexture);
        Shader.SetGlobalFloat(mapSizeId, mapSize);

        bufferElements = new List<EntityBufferElement>();
        ecosystemGrids = new int[(TextureSize * TextureSize) * 4];
    }

    private void Start()
    {
        Dictionary<Vector2Int, int> globalGrid = new Dictionary<Vector2Int, int>();

        for(int x = 0; x < TextureSize ; x++)
        {
            for (int y = 0; y < TextureSize; y++)
            {
                globalGrid.Add(new Vector2Int(x, y), 0);
            }
        }

        pixelsBuffer = new ComputeBuffer((TextureSize * TextureSize) * 4, sizeof(int));

        compute.SetBuffer(0, pixelBufferId, pixelsBuffer);
    }

    private void OnDestroy()
    {
        entityBuffer?.Dispose();
        pixelsBuffer?.Dispose();

        if (noiseTexture != null)
            DestroyImmediate(noiseTexture);
    }

    private void Update()
    {
        bufferElements.Clear();

        if (ecosystemAgents.Count > 0)
        {
            foreach (EcosystemAgent agent in ecosystemAgents)
            {
                EntityBufferElement element = new EntityBufferElement
                {
                    PositionX = agent.transform.position.x,
                    PositionY = agent.transform.position.z,
                    Range = agent.Range,
                    Noise = agent.UsedDataType() == EcosystemDataType.Noise ? agent.GetScore() : 0,
                    Pollution = agent.UsedDataType() == EcosystemDataType.Pollution ? agent.GetScore() : 0,
                    Flora = agent.UsedDataType() == EcosystemDataType.Flora ? agent.GetScore() : 0,
                    Fauna = agent.UsedDataType() == EcosystemDataType.Fauna ? agent.GetScore() : 0
                };
                bufferElements.Add(element);
            }

            entityBuffer = new ComputeBuffer(bufferElements.Count * 7, sizeof(float));

            compute.SetBuffer(0, entityBufferId, entityBuffer);

            entityBuffer.SetData(bufferElements);

            compute.SetInt(entityCountId, bufferElements.Count);

            compute.Dispatch(0, Mathf.CeilToInt(TextureSize / 8.0f), Mathf.CeilToInt(TextureSize / 8.0f), 1);

            pixelsBuffer.GetData(ecosystemGrids);
        }
    }
    public int GetScoreAtPosition(Vector2 position, EcosystemDataType scoreType)
    {
        return ecosystemGrids[((int)(position.x * TextureSize / MapSize) * TextureSize + (int)(position.y * TextureSize / MapSize)) * 4 + (int)scoreType];
    }

    public void AddAgent(EcosystemAgent toAdd)
    {
        if(!ecosystemAgents.Contains(toAdd))
        {
            ecosystemAgents.Add(toAdd);
        }
    }

    public void RemoveAgent(EcosystemAgent toRemove)
    {
        if (ecosystemAgents.Contains(toRemove))
        {
            ecosystemAgents.Remove(toRemove);
        }
    }

    public Vector2Int positions;

    [ContextMenu("Debug")]
    public void DebugTest()
    {
        Debug.Log("Nxt : ");
        Debug.Log((int)(positions.x * TextureSize / MapSize));
        Debug.Log((int)(positions.y * TextureSize / MapSize));
        //Debug.Log(ecosystemGrids[0].scoreGridArray[(int)(positions.x * TextureSize / MapSize) * TextureSize + (int)(positions.y * TextureSize / MapSize)]);
    }
}
