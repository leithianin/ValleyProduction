using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AreaManager : VLY_Singleton<AreaManager>
{
    /// Dimensions de la map
    [SerializeField] private Vector2 worldDimension;
    /// Taille de la grille
    [SerializeField] private float areaSize;
    [SerializeField] private bool showGizmos;

    /// Dimensions de la grille
    private static Vector2Int gridDimension;
    /// Layer mask contenant les AreaDisplay
    [SerializeField] private LayerMask areaDisplayMask;
    private Vector2 CenterPosition => new Vector2(transform.position.x, transform.position.z);

    [SerializeField] private List<AreaUpdater> allUpdaters = new List<AreaUpdater>();
    private int updaterIndex;
    [SerializeField] private int numberDataToUpdateInFrame;

    [SerializeField] private Transform treeScoreHandler;
    [SerializeField] private ADI_VegetationDisplayer treeScorePrefab;

    [SerializeField] private Transform animalScoreHandler;
    [SerializeField] private List<ADI_AnimalDisplayer> animalScorePrefab;
    [SerializeField] private List<ADI_AnimalDisplayer> allAnimalDisplayers = new List<ADI_AnimalDisplayer>(); //TEMP

    [SerializeField] private Transform chunkHandler;
    [SerializeField] private ChunkDisplayer chunkDisplayerPrefab;
    private List<ChunkDisplayer> allChunks;

    /// Liste de toutes les zones de la map
    private List<Area> areas = new List<Area>();

    [SerializeField] public UnityEvent OnCreateGrid;

    public static List<Area> Areas => instance.areas;

    /// Taille de la map (Getter)
    private static float AreaHeight => instance.areaSize;

    private void Start()
    {
        CreateGrid();

        for(int i = 0; i < areas.Count; i++)
        {
            for(int j = 0; j < areas[i].datas.Count; j++)
            {
                areas[i].datas[j].CalculateScore();
            }
        }
    }

    public static int GetAnimalInValley() //TEMP
    {
        int toReturn = 0;

        for(int i = 0; i < instance.allAnimalDisplayers.Count; i++)
        {
            toReturn += instance.allAnimalDisplayers[i].AnimalCount;
        }

        return toReturn;
    }

    [ContextMenu("Set Datas")]
    private void SetAreaDatas()
    {
        int k = 0;
        while (treeScoreHandler.childCount > 0 && k < 1000)
        {
            DestroyImmediate(treeScoreHandler.GetChild(0).gameObject);
            k++;
        }

        gridDimension = new Vector2Int(Mathf.RoundToInt(worldDimension.x / areaSize), Mathf.RoundToInt(worldDimension.y / areaSize));

        for (int i = 0; i < gridDimension.x; i++)
        {
            for (int j = 0; j < gridDimension.y; j++)
            {
                ADI_VegetationDisplayer vegetation = Instantiate(treeScorePrefab.gameObject, treeScoreHandler).GetComponent<ADI_VegetationDisplayer>();
                vegetation.transform.position = new Vector3(areaSize / 2f + areaSize * i, 0, areaSize / 2f + areaSize * j) + new Vector3(GetWorldPositionOffset().x, 0, GetWorldPositionOffset().y);
                vegetation.SetTrees();
            }
        }
    }

    [ContextMenu("Set Trees")]
    private void SetTrees()
    {
        int k = 0;
        while (treeScoreHandler.childCount > 0 && k < 1000)
        {
            DestroyImmediate(treeScoreHandler.GetChild(0).gameObject);
            k++;
        }

        gridDimension = new Vector2Int(Mathf.RoundToInt(worldDimension.x / areaSize), Mathf.RoundToInt(worldDimension.y / areaSize));

        for (int i = 0; i < gridDimension.x; i++)
        {
            for (int j = 0; j < gridDimension.y; j++)
            {
                ADI_VegetationDisplayer go = Instantiate(treeScorePrefab.gameObject, treeScoreHandler).GetComponent<ADI_VegetationDisplayer>();
                go.transform.position = new Vector3(areaSize / 2f + areaSize * i, 0, areaSize / 2f + areaSize * j) + new Vector3(GetWorldPositionOffset().x, 0, GetWorldPositionOffset().y);
                go.SetTrees();
            }
        }
    }

    [ContextMenu("Set Animals")]
    private void SetAnimals()
    {
        int k = 0;
        while (animalScoreHandler.childCount > 0 && k < 1000)
        {
            DestroyImmediate(animalScoreHandler.GetChild(0).gameObject);
            k++;
        }

        gridDimension = new Vector2Int(Mathf.RoundToInt(worldDimension.x / areaSize), Mathf.RoundToInt(worldDimension.y / areaSize));

        allAnimalDisplayers.Clear(); //TEMP

        for (int i = 0; i < gridDimension.x; i++)
        {
            for (int j = 0; j < gridDimension.y; j++)
            {
                for (int l = 0; l < animalScorePrefab.Count; l++)
                {
                    ADI_AnimalDisplayer go = Instantiate(animalScorePrefab[l].gameObject, animalScoreHandler).GetComponent<ADI_AnimalDisplayer>();
                    allAnimalDisplayers.Add(go); //TEMP
                    go.transform.position = new Vector3(areaSize / 2f + areaSize * i, 0, areaSize / 2f + areaSize * j) + new Vector3(GetWorldPositionOffset().x, 0, GetWorldPositionOffset().y);
                }
            }
        }
    }



    /// <summary>
    /// Crée la grille, et assigne tous les AreaDisplay aux zones.
    /// </summary>
    [ContextMenu("Create Grid")]
    private void CreateGrid()
    {
        areas = new List<Area>();

        gridDimension = new Vector2Int(Mathf.RoundToInt(worldDimension.x / areaSize), Mathf.RoundToInt(worldDimension.y / areaSize));

        for (int i = 0; i < gridDimension.x; i++)
        {
            for(int j = 0; j < gridDimension.y; j++)
            {
                Area newArea = new Area();
                newArea.arrayPosition = new Vector2Int(i, j);
                newArea.SetWorldPosition(new Vector2(areaSize / 2f + areaSize * i, areaSize / 2f + areaSize * j) + GetWorldPositionOffset());

                newArea.datas.Add(new AD_Noise());
                newArea.datas[newArea.datas.Count - 1].linkedArea = newArea;

                newArea.datas.Add(new AD_PlantHealthyness());
                newArea.datas[newArea.datas.Count - 1].linkedArea = newArea;

                newArea.datas.Add(new AD_Pollution());
                newArea.datas[newArea.datas.Count - 1].linkedArea = newArea;

                areas.Add(newArea);
            }
        }

        if (instance != null)
        {
            for (int i = 0; i < areas.Count; i++)
            {
                areas[i].SetAllDisplay(areaSize, areaDisplayMask);
            }
        }

        foreach(Area a in areas)
        { 
            HeatMapMaskRenderer.RegisterChunks(a); 
        }

        //OnCreateGrid?.Invoke();
    }

    /// <summary>
    /// Gère la mise à jour d'un nombre définit d'AreaUpdater
    /// </summary>
    private void LateUpdate()
    {
        if (allUpdaters.Count > 0)
        {
            for (int i = 0; i < numberDataToUpdateInFrame; i++)
            {
                updaterIndex = (updaterIndex + 1) % allUpdaters.Count;
                allUpdaters[updaterIndex].UpdateData();
            }
        }
    }

    /// <summary>
    /// Ajoute un AreaUpdater dans la liste.
    /// </summary>
    /// <param name="toAdd">L'AreaUpdater à ajouter.</param>
    public static void AddAreaUpdater(AreaUpdater toAdd)
    {
        if(!instance.allUpdaters.Contains(toAdd))
        {
            instance.allUpdaters.Add(toAdd);
        }
    }

    /// <summary>
    /// Retire un AreaUpdater de la liste.
    /// </summary>
    /// <param name="toAdd">L'AreaUpdater à retirer.</param>
    public static void RemoveAreaUpdater(AreaUpdater toRemove)
    {
        if (instance.allUpdaters.Contains(toRemove))
        {
            toRemove.RemoveData();

            instance.allUpdaters.Remove(toRemove);

            if(instance.allUpdaters.Count <= instance.updaterIndex)
            {
                instance.updaterIndex = instance.allUpdaters.Count;
            }
        }
    }

    private Vector2 GetWorldPositionOffset()
    {
        return CenterPosition - (worldDimension / 2f);
    }

    private int GetListIndexFromPosition(Vector2 position)
    {
        int columnIndex = Mathf.RoundToInt((position.x - areaSize / 2 - GetWorldPositionOffset().x) / areaSize);
        int lineIndex = Mathf.RoundToInt((position.y - areaSize / 2 - GetWorldPositionOffset().y) / areaSize);

        return columnIndex * gridDimension.y + lineIndex;
    }

    /// <summary>
    /// Permet de récupérer une Area depuis une position (x,z).
    /// </summary>
    /// <param name="position">Position de recherche (x,z).</param>
    /// <returns>L'Area dans laquelle la position se trouve.</returns>
    public static Area GetAreaAtPosition(Vector2 position)
    {
        int columnIndex = Mathf.RoundToInt((position.x - AreaHeight / 2 - instance.GetWorldPositionOffset().x) / AreaHeight);
        int lineIndex = Mathf.RoundToInt((position.y - AreaHeight / 2 - instance.GetWorldPositionOffset().y) / AreaHeight);

        return GetAreaAtIndex(new Vector2Int(columnIndex, lineIndex));
    }

    public static Area GetAreaAtIndex(Vector2Int index)
    {
        int realIndex = index.x * gridDimension.y + index.y;

        if (realIndex >= 0 && realIndex < Areas.Count)
        {
            return Areas[realIndex];
        }

        return null;
    }

    /// <summary>
    /// Ajoute une data à l'Area voulue.
    /// </summary>
    /// <typeparam name="T">Type de la data à ajouter.</typeparam>
    /// <param name="toUpdate">L'Area à update.</param>
    /// <param name="dataToUpdate">La data à ajouter.</param>
    public static void AddDataToArea<T>(Area toUpdate, T dataToUpdate)
    {
        if (toUpdate != null)
        {
            List<AreaData<T>> dataAreaToUpdate = toUpdate.GetData<T>();
            for (int i = 0; i < dataAreaToUpdate.Count; i++)
            {
                dataAreaToUpdate[i].AddData(dataToUpdate);
            }
        }
    }

    /// <summary>
    /// Retire une data à l'Area voulue.
    /// </summary>
    /// <typeparam name="T">Le type de data à retirer.</typeparam>
    /// <param name="toUpdate">L'Area à update.</param>
    /// <param name="dataToUpdate">La data à retirer.</param>
    public static void RemoveDataToArea<T>(Area toUpdate, T dataToUpdate)
    {
        if (toUpdate != null)
        {
            List<AreaData<T>> dataAreaToUpdate = toUpdate.GetData<T>();
            for (int i = 0; i < dataAreaToUpdate.Count; i++)
            {
                dataAreaToUpdate[i].RemoveData(dataToUpdate);
            }
        }
    }

    /// <summary>
    /// Update la data à l'Area voulue.
    /// </summary>
    /// <typeparam name="T">Le type de la data à update.</typeparam>
    /// <param name="toUpdate">L'Area à update.</param>
    /// <param name="dataToRemove">L'ancienne valeur de la data.</param>
    /// <param name="dataToAdd">La nouvelle valeur de la data.</param>
    public static void RefreshDataToArea<T>(Area toUpdate, T dataToRemove, T dataToAdd)
    {
        if (toUpdate != null)
        {
            List<AreaData<T>> dataAreaToUpdate = toUpdate.GetData<T>();
            for (int i = 0; i < dataAreaToUpdate.Count; i++)
            {
                dataAreaToUpdate[i].RefreshData(dataToRemove, dataToAdd);
            }
        }
    }

    public static List<Area> GetNeighbours(Area toCheck)
    {
        List<Area> neighbours = new List<Area>();

        for(int i = -1; i <= 1; i++)
        {
            for(int j = -1; j <= 1; j++)
            {
                if ((i == 0 || j == 0) && j != i)
                {
                    Vector2Int indexToCheck = new Vector2Int(i, j);
                    if (GetAreaAtIndex(toCheck.arrayPosition + indexToCheck) != null)
                    {
                        neighbours.Add(GetAreaAtIndex(toCheck.arrayPosition + indexToCheck));
                    }
                }
            }
        }

        return neighbours;
    }

    private void OnDrawGizmos()
    {
        if (showGizmos)
        {
            Gizmos.color = Color.yellow;
            for (int i = 0; i < areas.Count; i++)
            {
                Gizmos.DrawWireCube(areas[i].GetWorldPosition, Vector3.one * areaSize);
            }
        }
    }
}
