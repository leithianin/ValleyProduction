using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatmapViewController : MonoBehaviour
{
    [SerializeField] private MaskRenderer msk;

    public Material terrainMat;
    public MeshRenderer terrainRenderer;
    private List<Material> usedMat = new List<Material>();

    public Texture puf;

    public Material[] Materials;
    public GameObject baseLights;
    public GameObject heatmapLight;
    public GameObject foliage;

    private bool isEnabled;

    void Start()
    {
        terrainMat = Instantiate(terrainMat);

        terrainRenderer.material = terrainMat;

        usedMat.Add(terrainMat);

        foreach (Material m in Materials)
        {
            usedMat.Add(m);
        }

        foreach (Material m in usedMat)
        {
            m.SetTexture("_NoiseTex", msk.noiseTexture);
            m.SetTexture("_PollutionTex", msk.pollutionTexture);
            m.SetTexture("_FaunaTex", msk.faunaTexture);
            m.SetTexture("_FloraTex", msk.floraTexture);
        }

        //EnableHeatmapView(false, 0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            HandleHeatmap(1);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            HandleHeatmap(2);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            HandleHeatmap(3);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            HandleHeatmap(4);
        }

        foreach (Material m in usedMat)
        {
            m.SetFloat("_MapSize", msk.MapSize);
        }
    }

    private void OnApplicationQuit()
    {
        EnableHeatmapView(false, 0);
    }
    public void HandleHeatmap(int index)
    {
        EnableHeatmapView(!isEnabled, index);
    }

    private void EnableHeatmapView(bool enable, int index)
    {
        isEnabled = enable;

        baseLights.SetActive(!enable);
        heatmapLight.SetActive(enable);
        foliage.SetActive(!enable);

        foreach (Material m in usedMat)
        {
            if (enable) m.EnableKeyword("RENDER_HEATMAP");
            else m.DisableKeyword("RENDER_HEATMAP");

            //m.SetTexture("MainTex", msk.floraTexture);
            m.SetFloat("HEATMAP_INDEX", index);
        }
    }
}
