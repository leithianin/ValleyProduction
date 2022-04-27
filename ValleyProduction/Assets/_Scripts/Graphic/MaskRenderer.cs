using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct EcosystemGrid
{
    public EcosystemDataType scoreType;
    public int[] scoreGridArray;
}


public class MaskRenderer : MonoBehaviour
{
    [Obsolete]
    private static List<Entity> entities;
    [Obsolete]
    public static void RegisterEntity(Entity entity) { entities.Add(entity); }

    private List<EcosystemAgent> ecosystemAgents = new List<EcosystemAgent>();

    #region Properties
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
    }

    private List<EntityBufferElement> bufferElements;
    private ComputeBuffer entityBuffer = null;


    [SerializeField] private List<EcosystemGrid> ecosystemGrids = new List<EcosystemGrid>();

    private ComputeBuffer pixelsBuffer = null;
    #endregion

    public List<EcosystemGrid> Grids => ecosystemGrids;

    public List<EcosystemAgent> Agents => ecosystemAgents;

    private void Awake()
    {
        entities = new List<Entity>();

#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        maskTexture = new RenderTexture(TextureSize, TextureSize, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear)
#else
        noiseTexture = new RenderTexture(TextureSize, TextureSize, 0, RenderTextureFormat.ARGBFloat)
#endif
        {
            enableRandomWrite = true
        };
        noiseTexture.Create();

        compute.SetInt(textureSizeId, TextureSize);
        compute.SetTexture(0, noiseTextureId, noiseTexture);

        compute.SetFloat(blendId, BlendDistance);

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

        Shader.SetGlobalTexture(noiseTextureId, noiseTexture);
        Shader.SetGlobalFloat(mapSizeId, mapSize);

        bufferElements = new List<EntityBufferElement>();
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

        for (int i = 0; i < Enum.GetNames(typeof(EcosystemDataType)).Length; i ++)
        {
            ecosystemGrids.Add(new EcosystemGrid { scoreType = (EcosystemDataType)i , scoreGridArray = new int[(TextureSize * TextureSize)] }) ;
        }

        pixelsBuffer = new ComputeBuffer((TextureSize * TextureSize), sizeof(int));

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
                    Noise = agent.GetScore()
                };
                bufferElements.Add(element);
            }

            entityBuffer = new ComputeBuffer(bufferElements.Count * 4, sizeof(float));

            compute.SetBuffer(0, entityBufferId, entityBuffer);

            entityBuffer.SetData(bufferElements);

            compute.SetInt(entityCountId, bufferElements.Count);

            compute.Dispatch(0, Mathf.CeilToInt(TextureSize / 8.0f), Mathf.CeilToInt(TextureSize / 8.0f), 1);

            pixelsBuffer.GetData(ecosystemGrids[0].scoreGridArray);
        }
    }



    public int GetScoreAtPosition(Vector2 position, EcosystemDataType scoreType)
    {
        //HEATMAPSYST : Prendre en compte le ScoreType
        return ecosystemGrids[0].scoreGridArray[(int)(position.x * TextureSize / MapSize) * TextureSize + (int)(position.y * TextureSize / MapSize)];
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
        Debug.Log(ecosystemGrids[0].scoreGridArray[(int)(positions.x * TextureSize / MapSize) * TextureSize + (int)(positions.y * TextureSize / MapSize)]);
    }
}
