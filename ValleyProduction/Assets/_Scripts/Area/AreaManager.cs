using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : VLY_Singleton<AreaManager>
{
    /// Dimensions de la map
    [SerializeField] private Vector2 worldDimension;
    /// Taille de la grille
    [SerializeField] private float areaHeight;
    [SerializeField] private bool showGizmos;

    /// Dimensions de la grille
    private static Vector2Int gridDimension;
    /// Layer mask contenant les AreaDisplay
    [SerializeField] private LayerMask areaDisplayMask;

    private static List<AreaUpdater> allUpdaters = new List<AreaUpdater>();
    private static int updaterIndex;
    [SerializeField] private int numberSataToUpdateInFrame;

    /// Liste de toutes les zones de la map
    private static List<Area> areas = new List<Area>();

    /// Taille de la map (Getter)
    private static float AreaHeight => instance.areaHeight;

    private void Start()
    {
        CreateGrid();
    }

    /// <summary>
    /// Crée la grille, et assigne tous les AreaDisplay aux zones.
    /// </summary>
    [ContextMenu("Create Grid")]
    private void CreateGrid()
    {
        areas = new List<Area>();

        gridDimension = new Vector2Int(Mathf.RoundToInt(worldDimension.x / areaHeight), Mathf.RoundToInt(worldDimension.y / areaHeight));

        for (int i = 0; i < gridDimension.x; i++)
        {
            for(int j = 0; j < gridDimension.y; j++)
            {
                Area newArea = new Area();
                newArea.arrayPosition = new Vector2Int(i, j);
                newArea.worldPosition = new Vector2(areaHeight / 2f + areaHeight * i, areaHeight / 2f + areaHeight * j);

                newArea.datas.Add(new AD_Noise());

                areas.Add(newArea);
            }
        }

        if (instance != null)
        {
            for (int i = 0; i < areas.Count; i++)
            {
                areas[i].SetAllDisplay(areaHeight, areaDisplayMask);
            }
        }
    }

    /// <summary>
    /// Gère la mise à jour d'un nombre définit d'AreaUpdater
    /// </summary>
    private void LateUpdate()
    {
        Debug.Log(areas.Count);
        if (allUpdaters.Count > 0)
        {
            for (int i = 0; i < numberSataToUpdateInFrame; i++)
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
        if(!allUpdaters.Contains(toAdd))
        {
            allUpdaters.Add(toAdd);
        }
    }

    /// <summary>
    /// Retire un AreaUpdater de la liste.
    /// </summary>
    /// <param name="toAdd">L'AreaUpdater à retirer.</param>
    public static void RemoveAreaIpdater(AreaUpdater toRemove)
    {
        if (allUpdaters.Contains(toRemove))
        {
            allUpdaters.Remove(toRemove);

            if(allUpdaters.Count <= updaterIndex)
            {
                updaterIndex = allUpdaters.Count;
            }
        }
    }

    /// <summary>
    /// Permet de récupérer une Area depuis une position (x,z).
    /// </summary>
    /// <param name="position">Position de recherche (x,z).</param>
    /// <returns>L'Area dans laquelle la position se trouve.</returns>
    public static Area GetAreaAtPosition(Vector2 position)
    {
        int columnIndex = Mathf.RoundToInt((position.x - AreaHeight / 2) / AreaHeight);
        int lineIndex = Mathf.RoundToInt((position.y - AreaHeight / 2) / AreaHeight);

        int realIndex = columnIndex * gridDimension.y + lineIndex;

        if (realIndex >= 0 && realIndex < areas.Count)
        {
            return areas[realIndex];
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

    private void OnDrawGizmos()
    {
        if (showGizmos)
        {
            Gizmos.color = Color.yellow;
            for (int i = 0; i < areas.Count; i++)
            {
                Gizmos.DrawWireCube(areas[i].GetWorldPosition, Vector3.one * areaHeight);
            }
        }
    }
}
